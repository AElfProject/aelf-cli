using System;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands
{
    public class CallCommand : IAElfCommand, ITransientDependency
    {
        public const string Name = "call";
        
        private readonly IBlockChainService _blockChainService;
        
        public ILogger<CallCommand> Logger { get; set; }

        public CallCommand(IBlockChainService blockChainService)
        {
            _blockChainService = blockChainService;
            Logger = NullLogger<CallCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            // call AElf.ContractNames.Token -cm GetBalance -cp '{"owner":{ "value": "OT0klzwoNF0M2J2/H3QWLqDrmLytDoNCkPMa+I/hoMw=" },"symbol":"ELF"}'
            // call AElf.ContractNames.Token -m GetBalance -p '{"owner":{ "value": "0hHPcj8XOW988oLcA6JjaOgqAm1WmJMD81Zl7LQ8cIY=" },"symbol":"ELF"}'
            
            var method = commandLineArgs.Options.GetOrNull(Options.Method.Short, Options.Method.Long);
            if (string.IsNullOrWhiteSpace(commandLineArgs.Target))
            {
                throw new AElfCliUsageException(
                    "Contract name or address is missing!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }
            
            if (string.IsNullOrWhiteSpace(method))
            {
                throw new AElfCliUsageException(
                    "Contract method is missing!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }
            
            var @params = commandLineArgs.Options.GetOrNull(Options.Params.Short, Options.Params.Long);

            var result =
                await _blockChainService.ExecuteTransactionAsync(commandLineArgs.Target, method, @params);
            
            Logger.LogInformation("Result:");
            Logger.LogInformation(result);
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("Usage:");
            sb.AppendLine();
            sb.AppendLine("    aelf call <contract-name|contract-address> [options] ");
            sb.AppendLine();
            sb.AppendLine("Options:");
            sb.AppendLine();
            sb.AppendLine("    -cm|--method: The contract method to call.");
            sb.AppendLine("    -cp|--params: The contract params to call.");
            sb.AppendLine();
            sb.AppendLine("See the documentation for more info: https://docs.aelf.io");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Call a read-only method on a contract.";
        }
        
        public static class Options
        {
            public static class Method
            {
                public const string Short = "cm";
                public const string Long = "method";
            }
            
            public static class Params
            {
                public const string Short = "cp";
                public const string Long = "params";
            }
        }
    }
}