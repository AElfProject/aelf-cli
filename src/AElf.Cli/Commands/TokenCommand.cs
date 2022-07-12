using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Console.Core;
using AElf.Cli.Services;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands;

public class CreateTokenCommand : IAElfCommand, ITransientDependency
{
    public const string Name = "token";
    private readonly IBlockChainService _blockChainService;

    private readonly ITokenService _tokenService;
    private readonly IUserContext _userContext;
    private readonly IDisplayService _displayService;

    public CreateTokenCommand(ITokenService tokenService, IBlockChainService blockChainService,
        IUserContext userContext, IDisplayService displayService)
    {
        _tokenService = tokenService;
        _blockChainService = blockChainService;
        _userContext = userContext;
        _displayService = displayService;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var endpoint = _userContext.Endpoint;

        string method;
        if (string.IsNullOrWhiteSpace(commandLineArgs.Target))
        {
            var methodList = new List<string>
            {
                "Create", "Transfer", "TransferFrom", "Issue", "Approve", "UnApprove", "Burn",
                "GetTokenInfo", "GetBalance", "GetAllowance"
            };
            method = _displayService.Select(methodList);
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
                    var symbol = _displayService.Ask<string>("[blue]Symbol[/]:");
                    var tokenName = _displayService.Ask<string>("[blue]Token Name[/]:");
                    var totalSupply = _displayService.Ask<long>("[blue]Total Supply[/]:");
                    var decimals = _displayService.Ask<int>("[blue]Decimals[/]:");
                    var issuer = _displayService.Confirm($"Set {_userContext.Account} as the [blue]Issuer?[/]")
                        ? _userContext.Account
                        : _displayService.Ask<string>("[blue]Issuer[/]:");
                    var isBurnable = _displayService.Confirm("[blue]Is Burnable[/]:");
                    var issueChainId = _displayService.Prompt<string>("[blue]Issue Chain Id[/]", new List<string>
                        {
                            $"{AElfCliConstants.MainChainId}(AElf)",
                            $"{AElfCliConstants.SideChainId1}(tdVV)",
                            $"{AElfCliConstants.SideChainId2}(tdVW)"
                        }
                    );
                    issueChainId = issueChainId.Split('(').First().Trim();

                    var txId = await _tokenService.CreateAsync(
                        symbol,
                        tokenName.IsNullOrWhiteSpace() ? symbol : tokenName,
                        totalSupply,
                        decimals,
                        issuer.IsNullOrWhiteSpace() ? _userContext.Account : issuer,
                        isBurnable,
                        int.Parse(issueChainId));
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
}