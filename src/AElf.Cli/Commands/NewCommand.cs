using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Args.Validation.CommandValitators;
using AElf.Cli.Building.Project;
using AElf.Cli.Building.Template;
using AElf.Cli.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands;

public class NewCommand : IAElfCommand, ITransientDependency
{
    public const string Name = "new";

    private readonly IGeneratingService _generatingService;

    public NewCommand(IGeneratingService generatingService)
    {
        _generatingService = generatingService;
        Logger = NullLogger<NewCommand>.Instance;
    }

    public ILogger<NewCommand> Logger { get; set; }

    public Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        NewCommandValidator.Validate(commandLineArgs, GetUsageInfo());

        var newCommandArgs = new NewCommandArgs(commandLineArgs);

        var templateBuilder = new TemplateBuilder(newCommandArgs);
        var templateBuildResult = templateBuilder.Build();

        var projectBuilder = new ProjectBuilder(templateBuildResult, newCommandArgs); 
        projectBuilder.Build();
        
        Logger.LogInformation("Created successfully!");
        Logger.LogInformation("Directory: ${FullPath}", Path.GetFullPath(newCommandArgs.OutputFolder));
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
        sb.AppendLine("    -o|--output-folder: Specifies the output folder. Default value is the current directory.");
        sb.AppendLine();
        sb.AppendLine("Examples:");
        sb.AppendLine();
        sb.AppendLine("    aelf new YourCompany.YourProject");
        sb.AppendLine("    aelf new YourCompany.YourProject -o d:\\my-project");
        sb.AppendLine();
        sb.AppendLine("See the documentation for more info: https://docs.aelf.io");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Generates a new contract development solution based on the AElf Boilerplate templates.";
    }

    public static class NewCommandOptions
    {
        public static class OutputFolder
        {
            public const string Short = "o";
            public const string Long = "output-folder";
        }
        
        public static class TemplateVersion
        {
            public const string Short = "tv";
            public const string Long = "template-version";
        }
    }

    public class NewCommandArgs
    {
        public string Version { get; }
        public string ProjectName { get; }
        public string TemplateName { get; }
        public string OutputFolder { get; }

        public NewCommandArgs(CommandLineArgs args)
        {
            ProjectName = args.Target;
            
            var outputFolder = args.Options.GetOrNull(NewCommandOptions.OutputFolder.Short, NewCommandOptions.OutputFolder.Long);
            OutputFolder = outputFolder.IsNullOrWhiteSpace() ? Directory.GetCurrentDirectory() : outputFolder;
            
            var templateVersion = args.Options.GetOrNull(NewCommandOptions.TemplateVersion.Short, NewCommandOptions.TemplateVersion.Long);
            Version = templateVersion.IsNullOrWhiteSpace() ? "latest" : templateVersion;

            TemplateName = "app";
        }
    }
}