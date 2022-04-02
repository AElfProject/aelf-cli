using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using AElf.Cli.Args;
using AElf.Cli.Commands;
using AElf.Cli.Services;
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
                
                var endpoint = commandLineArgs.Options.GetOrNull(GlobalOptions.Endpoint.Short, GlobalOptions.Endpoint.Long);
                var account = commandLineArgs.Options.GetOrNull(GlobalOptions.Account.Short, GlobalOptions.Account.Long);
                var password = commandLineArgs.Options.GetOrNull(GlobalOptions.Password.Short, GlobalOptions.Password.Long);

                using var scope = ServiceScopeFactory.CreateScope();
                
                var context = scope.ServiceProvider.GetRequiredService<IUserContext>();
                context.Endpoint = endpoint;
                context.Account = account;
                context.Password = password;
                
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