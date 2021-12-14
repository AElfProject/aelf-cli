using System;
using System.Threading.Tasks;
using AElf.Client.Dto;
using AElf.Client.Service;
using AElf.Cryptography;
using AElf.Types;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services
{
    public interface IBlockChainService
    {
        Task<string> SendTransactionAsync(string contract, string method, string @params = null);

        Task<string> ExecuteTransactionAsync(string contract, string method,string @params = null);

        Task<string> CheckTransactionResultAsync(string txId);
    }

    public class BlockChainService : IBlockChainService, ITransientDependency
    {
        private readonly IUserContext _userContext;
        private readonly IAccountsService _accountsService;
        private readonly AElfClientFactory _aelfClientFactory;
        
        public ILogger<BlockChainService> Logger { get; set; }

        public BlockChainService(IUserContext userContext, IAccountsService accountsService, AElfClientFactory aelfClientFactory)
        {
            _userContext = userContext;
            _accountsService = accountsService;
            _aelfClientFactory = aelfClientFactory;

            Logger = NullLogger<BlockChainService>.Instance;
        }

        public async Task<string> SendTransactionAsync(string contract, string method, string @params = null)
        {
            var contractAddress = await GetContractAddressAsync(contract);
            var rawTransaction = await GenerateRawTransactionAsync(_userContext.Account, contractAddress, method, FormatParams(@params));
            var signature = await GetSignatureAsync(rawTransaction);

            var rawTransactionResult = await _aelfClientFactory.CreateClient().SendRawTransactionAsync(new SendRawTransactionInput()
            {
                Transaction = rawTransaction,
                Signature = signature,
            });

            Logger.LogInformation($"Send transaction: {rawTransactionResult.TransactionId} successfully.");
            
            return rawTransactionResult.TransactionId;
        }

        public async Task<string> ExecuteTransactionAsync(string contract, string method, string @params = null)
        {
            var contractAddress = await GetContractAddressAsync(contract);
            var rawTransaction = await GenerateRawTransactionAsync(_userContext.Account, contractAddress, method, FormatParams(@params));
            var signature = await GetSignatureAsync(rawTransaction);

            var rawTransactionResult = await _aelfClientFactory.CreateClient().ExecuteRawTransactionAsync(new ExecuteRawTransactionDto()
            {
                RawTransaction = rawTransaction,
                Signature = signature,
            });

            return rawTransactionResult;
        }

        public async Task<string> CheckTransactionResultAsync(string txId)
        {
            Logger.LogInformation("Checking transaction result...");
            
            var result = await _aelfClientFactory.CreateClient().GetTransactionResultAsync(txId);
            var i = 0;
            while (i < 4)
            {
                if (result.Status == TransactionResultStatus.Mined.ToString().ToUpper())
                {
                    Logger.LogInformation($"Transaction: {txId} executed successfully.");
                    return result.ReturnValue;
                }
                
                if (result.Status == TransactionResultStatus.Failed.ToString().ToUpper() || result.Status == TransactionResultStatus.NodeValidationFailed.ToString().ToUpper())
                {
                    Logger.LogInformation($"Transaction: {txId} failed. Error: {result.Error}");
                    return null;
                }

                await Task.Delay(1000);
                i++;
            }

            return null;
        }

        private async Task<string> GetSignatureAsync(string rawTransaction)
        {
            var transactionId = HashHelper.ComputeFrom(ByteArrayHelper.HexStringToByteArray(rawTransaction));
            var signature = await _accountsService.SignAsync(_userContext.Account, _userContext.Password,
                transactionId.ToByteArray());
            return ByteString.CopyFrom(signature).ToHex();
        }

        private async Task<string> GetContractAddressAsync(string contract)
        {
            var contractAddress = contract;
            if (contract.StartsWith("AElf.ContractNames."))
            {
                contractAddress = (await _aelfClientFactory.CreateClient().GetContractAddressByNameAsync(HashHelper.ComputeFrom(contract))).ToBase58();
            }

            return contractAddress;
        }

        private async Task<string> GenerateRawTransactionAsync(string from, string to,string method, string @params)
        {
            var status = await _aelfClientFactory.CreateClient().GetChainStatusAsync();
            var height = status.BestChainHeight;
            var blockHash = status.BestChainHash;
            
            var rawTransaction = await _aelfClientFactory.CreateClient().CreateRawTransactionAsync(new CreateRawTransactionInput
            {
                From = from,
                To = to,
                MethodName = method,
                Params = @params,
                RefBlockNumber = height,
                RefBlockHash = blockHash
            });

            return rawTransaction.RawTransaction;
        }

        private string FormatParams(string @params)
        {
            if (string.IsNullOrWhiteSpace(@params))
            {
                @params = "{}";
            }

            var json = JsonConvert.DeserializeObject(@params);
            return JsonConvert.SerializeObject(json);
        }
    }
}