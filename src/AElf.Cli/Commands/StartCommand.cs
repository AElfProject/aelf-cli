using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands
{
    public class StartCommand : IAElfCommand, ITransientDependency
    {
        public const string Name = "start";

        public ILogger<StartCommand> Logger { get; set; }

        public StartCommand()
        {
            Logger = NullLogger<StartCommand>.Instance;
        }

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Options.IsNullOrEmpty() || commandLineArgs.Options.ContainsKey(Options.DevSingleChain.Long))
            {
                Blockchain.Start();
            }

            return Task.CompletedTask;
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("'start' command is used to start an AElf blockchain node.");
            sb.AppendLine("See the documentation for more info: https://docs.aelf.io");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Start an AElf blockchain node on local machine.";
        }
        
        private static class Options
        {
            public static class DevSingleChain
            {
                public const string Long = "dev";
            }
        }
    }
}