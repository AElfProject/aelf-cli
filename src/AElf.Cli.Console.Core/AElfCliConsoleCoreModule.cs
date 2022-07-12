using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AElf.Cli.Console.Core;

[DependsOn(
    typeof(AbpAutofacModule)
)]
public class AElfCliConsoleCoreModule : AbpModule
{
    
}