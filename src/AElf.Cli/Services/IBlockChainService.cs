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
        Task<string> SendTransactionAsync(string contractAddress, string contractName, string method, string @params);

        Task<string> ExecuteTransactionAsync(string contractAddress, string contractName, string method,
            string @params);
    }

    public class BlockChainService : IBlockChainService, ITransientDependency
    {
        private readonly AElfClient _client;

        public BlockChainService()
        {
            // TODO: Get url from user config
            _client = new AElfClient("http://127.0.0.1:8000");
        }

        public async Task<string> SendTransactionAsync(string contractAddress, string contractName, string method, string @params)
        {
            // TODO: Get user key.
            var privateKey = "cd86ab6347d8e52bbbe8532141fc59ce596268143a308d1d40fedf385528b458";
            
            contractAddress = await GetContractAddressAsync(contractAddress, contractName);
            var rawTransaction = await GenerateRawTransactionAsync(privateKey, contractAddress, method, FormatParams(@params));
            var signature = GetSignature(privateKey,rawTransaction);

            var rawTransactionResult = await _client.SendRawTransactionAsync(new SendRawTransactionInput()
            {
                Transaction = rawTransaction,
                Signature = signature,
            });

            return rawTransactionResult.TransactionId;
        }

        public async Task<string> ExecuteTransactionAsync(string contractAddress, string contractName, string method, string @params)
        {
            // TODO: Get user key.
            var privateKey = "cd86ab6347d8e52bbbe8532141fc59ce596268143a308d1d40fedf385528b458";
            
            contractAddress = await GetContractAddressAsync(contractAddress, contractName);
            var rawTransaction = await GenerateRawTransactionAsync(privateKey, contractAddress, method, FormatParams(@params));
            var signature = GetSignature(privateKey,rawTransaction);

            var rawTransactionResult = await _client.ExecuteRawTransactionAsync(new ExecuteRawTransactionDto()
            {
                RawTransaction = rawTransaction,
                Signature = signature,
            });

            return rawTransactionResult;
        }
        
        private string GetSignature(string privateKey, string rawTransaction)
        {
            var transactionId = HashHelper.ComputeFrom(ByteArrayHelper.HexStringToByteArray(rawTransaction));
            var signature = CryptoHelper.SignWithPrivateKey(ByteArrayHelper.HexStringToByteArray(privateKey), transactionId.ToByteArray());
            return ByteString.CopyFrom(signature).ToHex();
        }

        private async Task<string> GetContractAddressAsync(string contractAddress, string contractName)
        {
            if (string.IsNullOrWhiteSpace(contractAddress))
            {
                contractAddress = (await _client.GetContractAddressByNameAsync(HashHelper.ComputeFrom(contractName))).ToBase58();
            }

            return contractAddress;
        }

        private async Task<string> GenerateRawTransactionAsync(string privateKey, string contractAddress,string method, string @params)
        {
            var address = _client.GetAddressFromPrivateKey(privateKey);
            var status = await _client.GetChainStatusAsync();
            var height = status.BestChainHeight;
            var blockHash = status.BestChainHash;
            
            var rawTransaction = await _client.CreateRawTransactionAsync(new CreateRawTransactionInput
            {
                From = address,
                To = contractAddress,
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