using CompanyName.ProjectName.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace CompanyName.ProjectName.EntityHandler.Core
{
    [DependsOn(
        typeof(ProjectNameApplicationModule),
        typeof(ProjectNameEntityFrameworkCoreModule),
        typeof(ProjectNameApplicationContractsModule)
    )]
    public class ProjectNameEntityHandlerCoreModule: AbpModule
    {
    }
}