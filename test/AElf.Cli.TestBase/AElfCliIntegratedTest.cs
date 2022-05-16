using MartinCostello.Logging.XUnit;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit.Abstractions;

namespace AElf.Cli.TestBase;

public class AElfCliIntegratedTest<TModule> : AbpIntegratedTest<TModule>
    where TModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected void SetTestOutputHelper(ITestOutputHelper testOutputHelper)
    {
        GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = testOutputHelper;
    }
}