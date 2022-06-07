using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands;

public class HelpCommand : IAElfCommand, ITransientDependency
{
    public const string Name = "help";

    public HelpCommand(IOptions<AElfCliOptions> walletOptions,
        IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
        AElfCliOptions = walletOptions.Value;

        Logger = NullLogger<HelpCommand>.Instance;
    }

    public ILogger<HelpCommand> Logger { get; set; }
    private AElfCliOptions AElfCliOptions { get; }
    private IServiceScopeFactory ServiceScopeFactory { get; }

    public Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        if (string.IsNullOrWhiteSpace(commandLineArgs.Target))
        {
            Logger.LogInformation("{UsageInfo}", GetUsageInfo());
            return Task.CompletedTask;
        }

        if (!AElfCliOptions.Commands.ContainsKey(commandLineArgs.Target))
        {
            Logger.LogWarning("There is no command named {Target}", commandLineArgs.Target);
            Logger.LogInformation("{UsageInfo}", GetUsageInfo());
            return Task.CompletedTask;
        }

        var commandType = AElfCliOptions.Commands[commandLineArgs.Target];

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var command = (IAElfCommand)scope.ServiceProvider.GetRequiredService(commandType);
            Logger.LogInformation("{UsageInfo}", command.GetUsageInfo());
        }

        return Task.CompletedTask;
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("");
        sb.AppendLine("    aelf <command> <target> [options]");
        sb.AppendLine("");
        sb.AppendLine("Command List:");
        sb.AppendLine("");

        foreach (var command in AElfCliOptions.Commands.ToArray())
        {
            string shortDescription;

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                shortDescription = ((IAElfCommand)scope.ServiceProvider
                    .GetRequiredService(command.Value)).GetShortDescription();
            }

            sb.Append("    > ");
            sb.Append(command.Key);
            sb.Append(string.IsNullOrWhiteSpace(shortDescription) ? "" : ":");
            sb.Append(' ');
            sb.AppendLine(shortDescription);
        }

        sb.AppendLine("");
        sb.AppendLine("To get a detailed help for a command:");
        sb.AppendLine("");
        sb.AppendLine("    aelf help <command>");
        sb.AppendLine("");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Show command line help. Write ` aelf help <command> `";
    }
}