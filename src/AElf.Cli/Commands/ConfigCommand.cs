using System;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands;

public class ConfigCommand : IAElfCommand, ITransientDependency
{
    public const string Name = "config";

    private readonly IConfigService _configService;

    public ConfigCommand(IConfigService configService)
    {
        _configService = configService;
        Logger = NullLogger<ConfigCommand>.Instance;
    }

    public ILogger<ConfigCommand> Logger { get; set; }

    public Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var key = commandLineArgs.Options.GetOrNull(Options.Key.Short, Options.Key.Long);
        var value = commandLineArgs.Options.GetOrNull(Options.Value.Short, Options.Value.Long);

        if (commandLineArgs.Target.IsNullOrWhiteSpace())
            throw new AElfCliUsageException(
                "Config command is missing!" +
                Environment.NewLine + Environment.NewLine +
                GetUsageInfo()
            );
        switch (commandLineArgs.Target!.Trim().ToLower())
        {
            case "list":
                var configs = _configService.GetList();
                foreach (var config in configs)
                    Logger.LogInformation("{Key}: {Value}", config.Key, config.Value);
                break;
            case "get":
                value = _configService.Get(key);
                Logger.LogInformation("{Key}: {Value}", key, value);
                break;
            case "set":
                var getResult = _configService.Set(key, value);
                Logger.LogInformation(getResult
                    ? $"The config: {key} is set successful."
                    : $"Failed to set config: {key}.");
                break;
            case "del":
                var delResult = _configService.Delete(key);
                Logger.LogInformation(delResult
                    ? $"The config: {key} is deleted successful."
                    : $"Failed to delete config: {key}.");
                break;
            default:
                throw new AElfCliUsageException(
                    $"Config command: {commandLineArgs.Target} is not supported!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo());
        }

        return Task.CompletedTask;
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("Usage:");
        sb.AppendLine();
        sb.AppendLine("    aelf config <command> [options]");
        sb.AppendLine();
        sb.AppendLine("    command: get, set, del or list.");
        sb.AppendLine();
        sb.AppendLine("Options:");
        sb.AppendLine();
        sb.AppendLine("    -k|--key:    The key of config item. (Supported keys: endpoint, account and password.)");
        sb.AppendLine("    -v|--value:  The value of config item.");
        sb.AppendLine();
        sb.AppendLine("Examples:");
        sb.AppendLine();
        sb.AppendLine("    aelf config get -k endpoint");
        sb.AppendLine("    aelf config set -k endpoint -v http://127.0.0.1:1235");
        sb.AppendLine("    aelf config del -k endpoint");
        sb.AppendLine("    aelf config list");
        sb.AppendLine();
        sb.AppendLine("See the documentation for more info: https://docs.aelf.io");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Get, set, delete or list aelf cli config.";
    }

    public static class Options
    {
        public static class Key
        {
            public const string Short = "k";
            public const string Long = "key";
        }

        public static class Value
        {
            public const string Short = "v";
            public const string Long = "value";
        }
    }
}