using System.IO;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Services;
using AElf.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands;

public class DeployCommand : IAElfCommand, ITransientDependency
{
    public const string Name = "deploy";

    private readonly IBlockChainService _blockChainService;

    public DeployCommand(IBlockChainService blockChainService)
    {
        _blockChainService = blockChainService;
        Logger = NullLogger<DeployCommand>.Instance;
    }

    public ILogger<DeployCommand> Logger { get; set; }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        int.TryParse(commandLineArgs.Options.GetOrNull(Options.Category.Short, Options.Category.Long),
            out var category);
        var codePath = commandLineArgs.Options.GetOrNull(Options.CodePath.Short, Options.CodePath.Long);
        var contractAddress =
            commandLineArgs.Options.GetOrNull(Options.ContractAddress.Short, Options.ContractAddress.Long);
        bool.TryParse(commandLineArgs.Options.GetOrNull(Options.UseProposal.Short, Options.UseProposal.Long),
            out var proposal);

        var code = await File.ReadAllBytesAsync(codePath);

        var txId = await _blockChainService.DeploySmartContractAsync(code, category, contractAddress, proposal);
        var result = await _blockChainService.CheckTransactionResultAsync(txId);

        if (result.Status == TransactionResultStatus.Mined.ToString().ToUpper())
            Logger.LogInformation(
                proposal
                    ? $"Smart contract submitted successfully! Proposal id is: {Hash.Parser.ParseFrom(ByteArrayHelper.HexStringToByteArray(result.ReturnValue)).ToHex()}"
                    : $"Smart contract deployed successfully! Contract address is: {Address.Parser.ParseFrom(ByteArrayHelper.HexStringToByteArray(result.ReturnValue)).ToBase58()}");
        else
            Logger.LogInformation(
                $"Smart contract deploy failed! Error: {result.Error}");
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("Usage:");
        sb.AppendLine();
        sb.AppendLine("    aelf deploy [options]");
        sb.AppendLine();
        sb.AppendLine("Options:");
        sb.AppendLine();
        //sb.AppendLine("    -c |--category:           Contract category.");
        sb.AppendLine("    -cp|--code-path:          Contract code file path.");
        sb.AppendLine(
            "    -ca|--contract-address:   Contract address. If you want to update a contract, you need to input the address of the contract to update.");
        sb.AppendLine("    -up|--use-proposal:       Whether to deploy the contract using the proposal.");
        sb.AppendLine();
        sb.AppendLine("Examples:");
        sb.AppendLine();
        sb.AppendLine("    aelf deploy -cp d:\\my-project\\MyContract.dll.patched");
        sb.AppendLine(
            "    aelf deploy -cp d:\\my-project\\MyContract-v2.dll.patched -ca XDrKT2syN...8UJym7YP9W9 -cp true");
        sb.AppendLine();
        sb.AppendLine("See the documentation for more info: https://docs.aelf.io");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Deploy or update a smart contract";
    }

    public static class Options
    {
        public static class Category
        {
            public const string Short = "c";
            public const string Long = "category";
        }

        public static class CodePath
        {
            public const string Short = "cp";
            public const string Long = "code-path";
        }

        public static class ContractAddress
        {
            public const string Short = "ca";
            public const string Long = "contract-address";
        }

        public static class UseProposal
        {
            public const string Short = "up";
            public const string Long = "use-proposal";
        }
    }
}