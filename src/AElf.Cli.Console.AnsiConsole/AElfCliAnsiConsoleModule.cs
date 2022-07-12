using AElf.Cli.Console.Core;
using Volo.Abp.Modularity;

namespace AElf.Cli.Console.AnsiConsole;

[DependsOn(
    typeof(AElfCliConsoleCoreModule)
)]
public class AElfCliAnsiConsoleModule : AbpModule
{
    
}