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
            // call -n AElf.ContractNames.Token -m GetBalance -p {"owner":{ "value": "OT0klzwoNF0M2J2/H3QWLqDrmLytDoNCkPMa+I/hoMw=" },"symbol":"ELF"}
            // call -n AElf.ContractNames.Token -m GetBalance -p "{\"owner\":{ \"value\": \"0hHPcj8XOW988oLcA6JjaOgqAm1WmJMD81Zl7LQ8cIY=\" },\"symbol\":\"ELF\"}"
            commandLineArgs.Options.TryGetValue(Options.ContractAddress, out var contractAddress);
            commandLineArgs.Options.TryGetValue(Options.ContractName, out var contractName);
            if (string.IsNullOrWhiteSpace(contractAddress) && string.IsNullOrWhiteSpace(contractName))
            {
                Logger.LogError("Either contract address or contract name must be entered!");
                return;
            }
            
            commandLineArgs.Options.TryGetValue(Options.Method, out var method);
            if (string.IsNullOrWhiteSpace(method) )
            {
                Logger.LogError("Method name is required!");
                return;
            }
            
            commandLineArgs.Options.TryGetValue(Options.Params, out var @params);

            var result =
                await _blockChainService.ExecuteTransactionAsync(contractAddress, contractName, method, @params);
            
            Logger.LogInformation("Result:");
            Logger.LogInformation(result);
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("Usage:");
            sb.AppendLine();
            sb.AppendLine("    aelf call [options] ");
            sb.AppendLine();
            sb.AppendLine("Options:");
            sb.AppendLine();
            sb.AppendLine("    -a: The address of the contract to call.");
            sb.AppendLine("    -n: The name of the contract to call.");
            sb.AppendLine("    -m: The method name of the contract to call.");
            sb.AppendLine("    -p: The method params of the contract to call.");
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
            public const string ContractAddress = "a";
            public const string ContractName = "n";
            public const string Method = "m";
            public const string Params = "p";
        }
    }
}