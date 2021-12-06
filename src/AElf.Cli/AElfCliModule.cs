using AElf.BIP39;
using AElf.Cli.Commands;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AElf.Cli
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(Bip39Module)
    )]
    public class AElfCliModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AElfCliOptions>(options =>
            {
                options.Commands[HelpCommand.Name] = typeof(HelpCommand);
                options.Commands[StartCommand.Name] = typeof(StartCommand);
                options.Commands[AccountsCommand.Name] = typeof(AccountsCommand);
                options.Commands[CreateCommand.Name] = typeof(CreateCommand);
                options.Commands[NewCommand.Name] = typeof(NewCommand);
                options.Commands[CallCommand.Name] = typeof(CallCommand);
                options.Commands[SendCommand.Name] = typeof(SendCommand);
                options.Commands[ConfigCommand.Name] = typeof(ConfigCommand);
            });
        }
    }
}