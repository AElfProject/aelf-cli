using System;
using System.Threading.Tasks;
using AElf.Client.Dto;
using AElf.Client.Service;
using AElf.Cryptography;
using AElf.Types;
using Google.Protobuf;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services
{
    public interface IBlockChainService
    {
        Task<string> SendTransactionAsync(string contract, string method, string @params);

        Task<string> ExecuteTransactionAsync(string contract, string method,string @params);
    }

    public class BlockChainService : IBlockChainService, ITransientDependency
    {
        private readonly AElfClient _client;
        private readonly IUserContext _userContext;
        private readonly IAccountsService _accountsService;

        public BlockChainService(IUserContext userContext, IAccountsService accountsService)
        {
            _userContext = userContext;
            _accountsService = accountsService;
            _client = new AElfClient(_userContext.GetEndpoint());
        }

        public async Task<string> SendTransactionAsync(string contract, string method, string @params)
        {
            var contractAddress = await GetContractAddressAsync(contract);
            var rawTransaction = await GenerateRawTransactionAsync(_userContext.GetAddress(), contractAddress, method, FormatParams(@params));
            var signature = await GetSignatureAsync(rawTransaction);

            var rawTransactionResult = await _client.SendRawTransactionAsync(new SendRawTransactionInput()
            {
                Transaction = rawTransaction,
                Signature = signature,
            });

            return rawTransactionResult.TransactionId;
        }

        public async Task<string> ExecuteTransactionAsync(string contract, string method, string @params)
        {
            var contractAddress = await GetContractAddressAsync(contract);
            var rawTransaction = await GenerateRawTransactionAsync(_userContext.GetAddress(), contractAddress, method, FormatParams(@params));
            var signature = await GetSignatureAsync(rawTransaction);

            var rawTransactionResult = await _client.ExecuteRawTransactionAsync(new ExecuteRawTransactionDto()
            {
                RawTransaction = rawTransaction,
                Signature = signature,
            });

            return rawTransactionResult;
        }
        
        private async Task<string> GetSignatureAsync(string rawTransaction)
        {
            var transactionId = HashHelper.ComputeFrom(ByteArrayHelper.HexStringToByteArray(rawTransaction));
            var signature = await _accountsService.SignAsync(_userContext.GetAddress(), _userContext.GetPassword(),
                transactionId.ToByteArray());
            return ByteString.CopyFrom(signature).ToHex();
        }

        private async Task<string> GetContractAddressAsync(string contract)
        {
            var contractAddress = contract;
            if (contract.StartsWith("AElf.ContractNames."))
            {
                contractAddress = (await _client.GetContractAddressByNameAsync(HashHelper.ComputeFrom(contract))).ToBase58();
            }

            return contractAddress;
        }

        private async Task<string> GenerateRawTransactionAsync(string from, string to,string method, string @params)
        {
            var status = await _client.GetChainStatusAsync();
            var height = status.BestChainHeight;
            var blockHash = status.BestChainHash;
            
            var rawTransaction = await _client.CreateRawTransactionAsync(new CreateRawTransactionInput
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
            var json = JsonConvert.DeserializeObject(@params);
            return JsonConvert.SerializeObject(json);
        }
    }
}