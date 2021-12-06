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
    public class NewCommand : IAElfCommand, ITransientDependency
    {
        public const string Name = "new";
        
        private readonly IGeneratingService _generatingService;
        
        public ILogger<NewCommand> Logger { get; set; }
        
        public NewCommand(IGeneratingService generatingService)
        {
            _generatingService = generatingService;
            Logger = NullLogger<NewCommand>.Instance;
        }
        
        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target.IsNullOrWhiteSpace())
            {
                Logger.LogInformation(GetUsageInfo());
                return Task.CompletedTask;
            }

            commandLineArgs.Options.TryGetValue(Options.OutputFolder, out var outputFolder);

            _generatingService.Generate(commandLineArgs.Target, outputFolder);
            return Task.CompletedTask;
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("Usage:");
            sb.AppendLine();
            sb.AppendLine("    aelf new <project-name> [options]");
            sb.AppendLine();
            sb.AppendLine(
                "    project-name: Common convention is to name a project is like YourCompany.YourProject. However, you can use different naming like YourProject (single level namespacing) or YourCompany.YourProduct.YourModule (three levels namespacing).");
            sb.AppendLine();
            sb.AppendLine("Options:");
            sb.AppendLine();
            sb.AppendLine("    -o: Specifies the output folder. Default value is the current directory.");
            sb.AppendLine();
            sb.AppendLine("See the documentation for more info: https://docs.aelf.io");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Generates a new contract development solution based on the AElf Boilerplate templates.";
        }
        
    }
    
    public static class Options
    {
        public const string OutputFolder = "o";
    }
}