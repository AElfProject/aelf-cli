using System;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands;

public class CreateTokenCommand : IAElfCommand, ITransientDependency
{
    public const string Name = "token";
    private readonly IBlockChainService _blockChainService;

    private readonly ITokenService _tokenService;
    private readonly IUserContext _userContext;

    public CreateTokenCommand(ITokenService tokenService, IBlockChainService blockChainService,
        IUserContext userContext)
    {
        _tokenService = tokenService;
        _blockChainService = blockChainService;
        _userContext = userContext;

        Logger = NullLogger<CreateTokenCommand>.Instance;
    }

    public ILogger<CreateTokenCommand> Logger { get; set; }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var symbol = commandLineArgs.Options.GetOrNull(Options.Symbol.Short, Options.Symbol.Long);
        var tokenName = commandLineArgs.Options.GetOrNull(Options.TokenName.Short, Options.TokenName.Long);
        var totalSupply = commandLineArgs.Options.GetOrNull(Options.TotalSupply.Short, Options.TotalSupply.Long);
        var decimals = commandLineArgs.Options.GetOrNull(Options.Decimals.Short, Options.Decimals.Long);
        var issuer = commandLineArgs.Options.GetOrNull(Options.Issuer.Short, Options.Issuer.Long);
        var isBurnable = commandLineArgs.Options.GetOrNull(Options.IsBurnable.Short, Options.IsBurnable.Long);
        var issuerChainId = commandLineArgs.Options.GetOrNull(Options.IssueChainId.Short, Options.IssueChainId.Long);

        var endpoint = _userContext.Endpoint;
        try
        {
            _userContext.Endpoint = AElfCliConstants.TestNetMainChainEndpoint;
            switch (commandLineArgs.Target?.ToLower())
            {
                case "create":
                    var txId = await _tokenService.CreateAsync(
                        symbol,
                        tokenName.IsNullOrWhiteSpace() ? symbol : tokenName,
                        totalSupply.IsNullOrWhiteSpace() ? 100000000000000000 : long.Parse(totalSupply),
                        decimals.IsNullOrWhiteSpace() ? 8 : int.Parse(decimals),
                        issuer.IsNullOrWhiteSpace() ? _userContext.Account : issuer,
                        isBurnable.IsNullOrWhiteSpace() ? true : bool.Parse(isBurnable),
                        issuerChainId.IsNullOrWhiteSpace() ? AElfCliConstants.AElfChainId : int.Parse(issuerChainId));
                    await _blockChainService.CheckTransactionResultAsync(txId);
                    break;
                default:
                    throw new AElfCliUsageException(
                        $"Token command: {commandLineArgs.Target} is not supported!" +
                        Environment.NewLine + Environment.NewLine +
                        GetUsageInfo());
            }
        }
        finally
        {
            _userContext.Endpoint = endpoint;
        }
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("Usage:");
        sb.AppendLine();
        sb.AppendLine("    aelf token <command> [options]");
        sb.AppendLine();
        sb.AppendLine("    command: create");
        sb.AppendLine();
        sb.AppendLine("Options:");
        sb.AppendLine();
        sb.AppendLine("    -s|--symbol: Token symbol that you want to create. Cannot be null or empty.");
        sb.AppendLine("    -tn|--tokenName: Token name that you want to create. Default value is same to symbol.");
        sb.AppendLine(
            "    -ts|--totalSupply: Total supply of the token that you want to create. Default value is 100000000000000000.");
        sb.AppendLine("    -d|--decimals: Decimals of the token that you want to create. Default value is 8.");
        sb.AppendLine(
            "    -i|--issuer: Issuer of the token that you want to create. Default value is your configured account.");
        sb.AppendLine("    -ib|--isBurnable: Is the token that you want to create burnable. Default value is True.");
        sb.AppendLine(
            "    -ici|--issueChainId: Issue Chain Id of the token that you want to create. Default value is 9992731.");
        sb.AppendLine();
        sb.AppendLine("Examples:");
        sb.AppendLine();
        sb.AppendLine("    aelf token create");
        sb.AppendLine("    aelf token create -s ELF1 -tn \"Elf token\" -ts 100000000000000000 -d 8 -ib true");
        sb.AppendLine();
        sb.AppendLine("See the documentation for more info: https://docs.aelf.io");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Create a new token in AElf MultiToken Contract.";
    }

    public static class Options
    {
        public static class Symbol
        {
            public const string Short = "s";
            public const string Long = "symbol";
        }

        public static class TokenName
        {
            public const string Short = "tn";
            public const string Long = "tokenName";
        }

        public static class TotalSupply
        {
            public const string Short = "ts";
            public const string Long = "totalSupply";
        }

        public static class Decimals
        {
            public const string Short = "d";
            public const string Long = "decimals";
        }

        public static class Issuer
        {
            public const string Short = "i";
            public const string Long = "issuer";
        }

        public static class IsBurnable
        {
            public const string Short = "ib";
            public const string Long = "isBurnable";
        }

        public static class IssueChainId
        {
            public const string Short = "ici";
            public const string Long = "issueChainId";
        }
    }
}