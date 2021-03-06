using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit.Abstractions;

namespace AElf.Cli.TestBase;

[DependsOn(
    typeof(AbpTestBaseModule))]
public class TestBaseAElfModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ITestOutputHelperAccessor testOutputHelperAccessor = new TestOutputHelperAccessor();

        context.Services.AddSingleton(testOutputHelperAccessor);

        context.Services.AddLogging(o =>
        {
            o.AddXUnit(testOutputHelperAccessor);
                 
        });
    }
}

public class TestOutputHelperAccessor : ITestOutputHelperAccessor
{
    public ITestOutputHelper OutputHelper { get; set; }
}