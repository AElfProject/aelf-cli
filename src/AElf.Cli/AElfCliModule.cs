using System.Globalization;
using AElf.BIP39;
using AElf.Cli.Commands;
using AElf.Cli.Console.AnsiConsole;
using AElf.Client.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AElf.Cli;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(Bip39Module),
    typeof(AElfClientModule),
    typeof(AElfCliAnsiConsoleModule)
)]
public class AElfCliModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
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
            options.Commands[FaucetCommand.Name] = typeof(FaucetCommand);
            options.Commands[TopOfOasisCommand.Name] = typeof(TopOfOasisCommand);
            options.Commands[DeployCommand.Name] = typeof(DeployCommand);
            options.Commands[CreateTokenCommand.Name] = typeof(CreateTokenCommand);
        });
    }
}