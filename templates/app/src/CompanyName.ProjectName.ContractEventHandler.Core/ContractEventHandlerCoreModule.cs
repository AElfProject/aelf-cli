using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.Core;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace CompanyName.ProjectName.ContractEventHandler.Core
{
    [DependsOn(
        typeof(ProjectNameApplicationModule),
        typeof(AElfNodeEventHandlerCoreModule)
        //typeof(AElfEventHandlerBackgroundJobModule)
    )]
    public class ContractEventHandlerCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ContractEventHandlerCoreModule>();
            });
        }
    }
}