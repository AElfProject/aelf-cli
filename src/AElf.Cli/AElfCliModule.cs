using AElf.Cli.Commands;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AElf.Cli
{
    [DependsOn(
        typeof(AbpAutofacModule)
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
            });
        }
    }
}