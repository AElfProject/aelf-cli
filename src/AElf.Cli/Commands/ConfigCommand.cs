using System;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mono.Cecil;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands
{
    public class ConfigCommand : IAElfCommand, ITransientDependency
    {
        public const string Name = "config";

        private readonly IConfigService _configService;
        
        public ILogger<ConfigCommand> Logger { get; set; }
        
        public ConfigCommand(IConfigService configService)
        {
            _configService = configService;
            Logger = NullLogger<ConfigCommand>.Instance;
        }

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target.IsNullOrWhiteSpace())
            {
                Logger.LogInformation(GetUsageInfo());
                return Task.CompletedTask;
            }

            commandLineArgs.Options.TryGetValue(Options.Key, out var key);
            commandLineArgs.Options.TryGetValue(Options.Value, out var value);
            
            switch (commandLineArgs.Target)
            {
                case "list":
                    var configs = _configService.GetList();
                    foreach (var config in configs)
                    {
                        Logger.LogInformation($"{config.Key}: {config.Value}");
                    }
                    break;
                case "get":
                    value = _configService.Get(key);
                    Logger.LogInformation($"{key}: {value}");
                    break;
                case "set":
                    _configService.Set(key, value);
                    break;
                case "del":
                    _configService.Delete(key);
                    break;
                default:
                    Logger.LogInformation(GetUsageInfo());
                    break;
            }

            Logger.LogInformation("Success!");
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
            sb.AppendLine("    -k: The key of config item. (Supported keys: endpoint, account and password.)");
            sb.AppendLine("    -v: The value of config item.");
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
            public const string Key = "k";
            public const string Value = "v";
        }
    }
}