using System;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands;

public class FaucetCommand : IAElfCommand, ITransientDependency
{
    public const string Name = "faucet";
    private readonly IBlockChainService _blockChainService;

    private readonly IFaucetService _faucetService;
    private readonly IUserContext _userContext;

    public FaucetCommand(IFaucetService faucetService, IBlockChainService blockChainService, IUserContext userContext)
    {
        _faucetService = faucetService;
        _blockChainService = blockChainService;
        _userContext = userContext;

        Logger = NullLogger<FaucetCommand>.Instance;
    }

    public ILogger<FaucetCommand> Logger { get; set; }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var symbol = commandLineArgs.Options.GetOrNull(Options.Symbol.Short, Options.Symbol.Long);
        var count = commandLineArgs.Options.GetOrNull(Options.Count.Short, Options.Count.Long);

        var endpoint = _userContext.Endpoint;
        try
        {
            _userContext.Endpoint = AElfCliConstants.TestNetMainChainEndpoint;
            switch (commandLineArgs.Target?.ToLower())
            {
                case "take":
                    var txId = await _faucetService.TakeAsync(
                        symbol.IsNullOrWhiteSpace() ? AElfCliConstants.AElfNativeSymbol : symbol,
                        count.IsNullOrWhiteSpace() ? 100_00000000 : long.Parse(count));
                    await _blockChainService.CheckTransactionResultAsync(txId);
                    break;
                default:
                    throw new AElfCliUsageException(
                        $"Faucet command: {commandLineArgs.Target} is not supported!" +
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
        sb.AppendLine("    aelf faucet <command> [options]");
        sb.AppendLine();
        sb.AppendLine("    command: take");
        sb.AppendLine();
        sb.AppendLine("Options:");
        sb.AppendLine();
        sb.AppendLine("    -s|--symbol: Token symbol that you want to operate on. Default value is ELF.");
        sb.AppendLine(
            "    -c|--count:  Token amount that you want to operate on. Default value is 100_00000000 (100 ELF).");
        sb.AppendLine();
        sb.AppendLine("Examples:");
        sb.AppendLine();
        sb.AppendLine("    aelf faucet take");
        sb.AppendLine("    aelf faucet take -s ELF -c 10000000000");
        sb.AppendLine();
        sb.AppendLine("See the documentation for more info: https://docs.aelf.io");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Take test token from AElf Test Net.";
    }

    public static class Options
    {
        public static class Symbol
        {
            public const string Short = "s";
            public const string Long = "symbol";
        }

        public static class Count
        {
            public const string Short = "c";
            public const string Long = "count";
        }
    }
}