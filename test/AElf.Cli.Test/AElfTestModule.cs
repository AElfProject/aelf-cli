using Volo.Abp.Modularity;

namespace AElf.Cli.Test;

[DependsOn(typeof(AElfCliModule))]
public class AElfTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}