using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AElf.Cli.Commands
{
    public class AccountsCommand : IConsoleCommand
    {
        public const string Name = "accounts";

        private readonly IAccountsService _accountsService;

        protected IServiceScopeFactory ServiceScopeFactory { get; }

        public ILogger<AccountsCommand> Logger { get; set; }

        public AccountsCommand(IAccountsService accountsService, IServiceScopeFactory serviceScopeFactory)
        {
            _accountsService = accountsService;
            ServiceScopeFactory = serviceScopeFactory;
            Logger = NullLogger<AccountsCommand>.Instance;
        }

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Options.ContainsKey(Options.List.Short) ||
                commandLineArgs.Options.ContainsKey(Options.List.Long))
            {
                Logger.LogInformation(_accountsService.GetLocalAccount().Aggregate("\n", (l, r) => $"{l}\n{r}"));
            }

            return Task.CompletedTask;
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine(
                "'account' command is used to manage your accounts (keys) that you can use to interact with AElf blockchains.");
            sb.AppendLine("See the documentation for more info: https://docs.aelf.io");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Manage local aelf blockchain accounts.";
        }

        private static class Options
        {
            public static class List
            {
                public const string Short = "l";
                public const string Long = "list";
            }
        }
    }
}