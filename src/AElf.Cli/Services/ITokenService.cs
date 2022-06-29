using System;
using System.Threading.Tasks;
using AElf.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services;

public interface ITokenService
{
    Task<string> CreateAsync(string symbol, string tokenName, long totalSupply, int decimals, string issuer,
        bool isBurnable, int issueChainId);

    Task<string> CrossChainCreateAsync(string symbol);
}

public class TokenService : ITokenService, ITransientDependency
{
    private readonly IBlockChainService _blockChainService;

    public TokenService(IBlockChainService blockChainService)
    {
        _blockChainService = blockChainService;
    }

    public async Task<string> CreateAsync(string symbol, string tokenName, long totalSupply, int decimals,
        string issuer, bool isBurnable, int issueChainId)
    {
        var @params = new JObject
        {
            ["symbol"] = symbol,
            ["tokenName"] = tokenName,
            ["totalSupply"] = totalSupply,
            ["decimals"] = decimals,
            ["issuer"] = new JObject
            {
                ["value"] = Address.FromBase58(issuer).Value.ToBase64()
            },
            ["isBurnable"] = isBurnable,
            ["issueChainId"] = issueChainId
        };

        return await _blockChainService.SendTransactionAsync(AElfCliConstants.TestMainChainTokenContractAddress,
            "Create",
            JsonConvert.SerializeObject(@params));
    }

    public Task<string> CrossChainCreateAsync(string symbol)
    {
        throw new NotImplementedException();
    }
}