using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using AElf.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands
{
    public interface ICommandSelector
    {
        Type Select(CommandLineArgs commandLineArgs);
    }

    public class CommandSelector : ICommandSelector, ITransientDependency
    {
        private AElfCliOptions Options { get; }

        public CommandSelector(IOptions<AElfCliOptions> options)
        {
            Options = options.Value;
        }

        public Type Select(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Command.IsNullOrWhiteSpace())
            {
                return typeof(HelpCommand);
            }

            return Options.Commands.GetOrDefault(commandLineArgs.Command)
                   ?? typeof(HelpCommand);
        }
    }
}