using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Spectre.Console;
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

        var endpoint = _userContext.Endpoint;

        string method;
        if (string.IsNullOrWhiteSpace(commandLineArgs.Target))
        {
            method = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(AElfCliConstants.ChooseAMethod)
                    .PageSize(AElfCliConstants.DefaultPageSize)
                    .MoreChoicesText(AElfCliConstants.MoreMethods)
                    .AddChoices("Create", "Transfer", "TransferFrom", "Issue", "Approve", "UnApprove", "Burn",
                        "GetTokenInfo", "GetBalance", "GetAllowance"));
        }
        else
        {
            method = commandLineArgs.Target?.ToLower();
        }

        try
        {
            _userContext.Endpoint = AElfCliConstants.TestNetMainChainEndpoint;
            switch (method)
            {
                case "Create":
                    var symbol = AnsiConsole.Ask<string>("[blue]Symbol[/]:");
                    var tokenName = AnsiConsole.Ask<string>("[blue]Token Name[/]:");
                    var totalSupply = AnsiConsole.Ask<long>("[blue]Total Supply[/]:");
                    var decimals = AnsiConsole.Ask<int>("[blue]Decimals[/]:");
                    var issuer = AnsiConsole.Confirm($"Set {_userContext.Account} as the [blue]Issuer?[/]")
                        ? _userContext.Account
                        : AnsiConsole.Ask<string>("[blue]Issuer[/]:");
                    var isBurnable = AnsiConsole.Confirm("[blue]Is Burnable[/]:");
                    var issuerChainId = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[blue]Issue Chain Id[/]")
                            .PageSize(AElfCliConstants.DefaultPageSize)
                            .AddChoices($"AElf: {AElfCliConstants.MainChainId}",
                                $"tdVV: {AElfCliConstants.SideChainId1}",
                                $"tdVW: {AElfCliConstants.SideChainId2}"
                            )
                            .UseConverter(o => o.Split(':').Last().Trim())
                    );

                    var txId = await _tokenService.CreateAsync(
                        symbol,
                        tokenName.IsNullOrWhiteSpace() ? symbol : tokenName,
                        totalSupply,
                        decimals,
                        issuer.IsNullOrWhiteSpace() ? _userContext.Account : issuer,
                        isBurnable,
                        int.Parse(issuerChainId));
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