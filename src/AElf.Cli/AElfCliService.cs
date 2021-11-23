using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using AElf.Cli.Args;
using AElf.Cli.Commands;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli
{
    public class AElfCliService : ITransientDependency
    {
        public ILogger<AElfCliService> Logger { get; set; }
        private ICommandLineArgumentParser CommandLineArgumentParser { get; }
        private ICommandSelector CommandSelector { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }

        public AElfCliService(ICommandLineArgumentParser commandLineArgumentParser, ICommandSelector commandSelector,
            IServiceScopeFactory serviceScopeFactory)
        {
            CommandLineArgumentParser = commandLineArgumentParser;
            CommandSelector = commandSelector;
            ServiceScopeFactory = serviceScopeFactory;

            Logger = NullLogger<AElfCliService>.Instance;
        }

        public async Task RunAsync(string[] args)
        {
            Logger.LogInformation("AElf Cli (https://aelf.io)");

            var commandLineArgs = CommandLineArgumentParser.Parse(args);

            try
            {
                var commandType = CommandSelector.Select(commandLineArgs);

                using var scope = ServiceScopeFactory.CreateScope();
                var command = (IAElfCommand) scope.ServiceProvider.GetRequiredService(commandType);
                await command.ExecuteAsync(commandLineArgs);
            }
            catch (AElfCliUsageException usageException)
            {
                Logger.LogWarning(usageException.Message);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}