using System.Threading.Tasks;
using AElf.Client.Dto;
using AElf.Types;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services;

public interface IBlockChainService
{
    Task<string> SendTransactionAsync(string contract, string method, string @params = null,
        string endpoint = null);

    Task<string> ExecuteTransactionAsync(string contract, string method, string @params = null,
        string endpoint = null);

    Task<TransactionResultDto> CheckTransactionResultAsync(string txId, string endpoint = null);

    Task<MerklePathDto> GetMerklePathByTransactionIdAsync(string transactionId, string endpoint = null);

    Task<TransactionResultDto> GetTransactionResultAsync(string transactionId, string endpoint = null);

    Task<string> DeploySmartContractAsync(byte[] codes, int category = 0, string contractAddress = null,
        bool createProposal = false, string endpoint = null);

    Task<string> GenerateRawTransactionAsync(string from, string to, string methodName, string @params,
        string endpoint = null);

    Task<(string, string)> SendTransactionReturnRawAsync(string contract, string method,
        string @params = null, string endpoint = null);

    Task<string> GetContractAddressByNameAsync(string contractName, string endpoint = null);
}

public class BlockChainService : IBlockChainService, ITransientDependency
{
    private readonly IAccountsService _accountsService;
    private readonly AElfClientFactory _aelfClientFactory;
    private readonly IUserContext _userContext;

    public BlockChainService(IUserContext userContext, IAccountsService accountsService,
        AElfClientFactory aelfClientFactory)
    {
        _userContext = userContext;
        _accountsService = accountsService;
        _aelfClientFactory = aelfClientFactory;

        Logger = NullLogger<BlockChainService>.Instance;
    }

    public ILogger<BlockChainService> Logger { get; set; }

    public async Task<string> SendTransactionAsync(string contract, string method, string @params = null,
        string endpoint = null)
    {
        var contractAddress = await GetContractAddressAsync(contract);
        var rawTransaction = await GenerateRawTransactionAsync(_userContext.Account, contractAddress, method,
            FormatParams(@params), endpoint);
        var signature = await GetSignatureAsync(rawTransaction);

        var rawTransactionResult = await _aelfClientFactory.CreateClient(endpoint).SendRawTransactionAsync(
            new SendRawTransactionInput
            {
                Transaction = rawTransaction,
                Signature = signature
            });

        Logger.LogInformation($"Send transaction: {rawTransactionResult.TransactionId} successfully.");

        return rawTransactionResult.TransactionId;
    }

    public async Task<(string, string)> SendTransactionReturnRawAsync(string contract, string method,
        string @params = null, string endpoint = null)
    {
        var contractAddress = await GetContractAddressAsync(contract);
        var rawTransaction = await GenerateRawTransactionAsync(_userContext.Account, contractAddress, method,
            FormatParams(@params), endpoint);
        var signature = await GetSignatureAsync(rawTransaction);

        var rawTransactionResult = await _aelfClientFactory.CreateClient(endpoint).SendRawTransactionAsync(
            new SendRawTransactionInput
            {
                Transaction = rawTransaction,
                Signature = signature
            });

        Logger.LogInformation($"Send transaction: {rawTransactionResult.TransactionId} successfully.");

        return (rawTransactionResult.TransactionId, rawTransaction);
    }

    public async Task<string> GetContractAddressByNameAsync(string contractName, string endpoint = null)
    {
        return (await _aelfClientFactory.CreateClient(endpoint)
            .GetContractAddressByNameAsync(HashHelper.ComputeFrom(contractName))).ToBase58();
    }

    public async Task<string> ExecuteTransactionAsync(string contract, string method, string @params = null,
        string endpoint = null)
    {
        var contractAddress = await GetContractAddressAsync(contract);
        var rawTransaction =
            await GenerateRawTransactionAsync(_userContext.Account, contractAddress, method, FormatParams(@params));
        var signature = await GetSignatureAsync(rawTransaction);

        var rawTransactionResult = await _aelfClientFactory.CreateClient(endpoint).ExecuteRawTransactionAsync(
            new ExecuteRawTransactionDto
            {
                RawTransaction = rawTransaction,
                Signature = signature
            });

        return rawTransactionResult;
    }

    public async Task<TransactionResultDto> CheckTransactionResultAsync(string txId, string endpoint = null)
    {
        Logger.LogInformation("Checking transaction result...");
        var client = _aelfClientFactory.CreateClient(endpoint);

        var result = await client.GetTransactionResultAsync(txId);
        var i = 0;
        while (i < 10)
        {
            if (result.Status == TransactionResultStatus.Mined.ToString().ToUpper())
            {
                Logger.LogInformation($"Transaction: {txId} executed successfully.");
                break;
            }

            if (result.Status == TransactionResultStatus.Failed.ToString().ToUpper() || result.Status ==
                TransactionResultStatus.NodeValidationFailed.ToString().ToUpper())
            {
                Logger.LogWarning($"Transaction: {txId} failed. Error: {result.Error}");
                break;
            }

            await Task.Delay(1000);
            result = await client.GetTransactionResultAsync(txId);
            i++;
        }

        return result;
    }

    public Task<MerklePathDto> GetMerklePathByTransactionIdAsync(string transactionId, string endpoint = null)
    {
        var client = _aelfClientFactory.CreateClient(endpoint);
        client.CreateRawTransactionAsync(new CreateRawTransactionInput());
        return client.GetMerklePathByTransactionIdAsync(transactionId);
    }

    public async Task<TransactionResultDto> GetTransactionResultAsync(string transactionId, string endpoint = null)
    {
        var client = _aelfClientFactory.CreateClient(endpoint);
        return await client.GetTransactionResultAsync(transactionId);
    }

    public async Task<string> DeploySmartContractAsync(byte[] codes, int category = 0,
        string contractAddress = null, bool createProposal = false, string endpoint = null)
    {
        var client = _aelfClientFactory.CreateClient(endpoint);
        var chain = await client.GetChainStatusAsync();

        if (string.IsNullOrWhiteSpace(contractAddress))
        {
            var @params = new JObject
            {
                ["category"] = category,
                ["code"] = ByteString.CopyFrom(codes).ToBase64()
            };
            return await SendTransactionAsync(chain.GenesisContractAddress,
                createProposal ? "ProposeNewContract" : "DeploySmartContract",
                JsonConvert.SerializeObject(@params));
        }
        else
        {
            var @params = new JObject
            {
                ["code"] = ByteString.CopyFrom(codes).ToBase64(),
                ["address"] = new JObject { ["value"] = Address.FromBase58(contractAddress).Value.ToBase64() }
            };
            return await SendTransactionAsync(chain.GenesisContractAddress,
                createProposal ? "ProposeUpdateContract" : "UpdateSmartContract",
                JsonConvert.SerializeObject(@params));
        }
    }

    public async Task<string> GenerateRawTransactionAsync(string from, string to, string method, string @params,
        string endpoint = null)
    {
        var client = _aelfClientFactory.CreateClient(endpoint);
        var status = await client.GetChainStatusAsync();
        var height = status.BestChainHeight;
        var blockHash = status.BestChainHash;

        Logger.LogInformation($"Actual params: {@params}");

        var rawTransaction = await client.CreateRawTransactionAsync(new CreateRawTransactionInput
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

    private async Task<string> GetSignatureAsync(string rawTransaction)
    {
        var transactionId = HashHelper.ComputeFrom(ByteArrayHelper.HexStringToByteArray(rawTransaction));
        var signature = await _accountsService.SignAsync(_userContext.Account, _userContext.Password,
            transactionId.ToByteArray());
        return ByteString.CopyFrom(signature).ToHex();
    }

    private async Task<string> GetContractAddressAsync(string contract, string endpoint = null)
    {
        var contractAddress = contract;
        if (contract.StartsWith("AElf.ContractNames."))
            contractAddress = (await _aelfClientFactory.CreateClient(endpoint)
                .GetContractAddressByNameAsync(HashHelper.ComputeFrom(contract))).ToBase58();

        return contractAddress;
    }

    private string FormatParams(string @params)
    {
        Logger.LogInformation($"Formatting params: {@params}");
        if (string.IsNullOrWhiteSpace(@params)) @params = "{}";

        var json = JsonConvert.DeserializeObject(@params);
        return JsonConvert.SerializeObject(json);
    }
}