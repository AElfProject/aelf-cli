using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Modularity;

namespace CompanyName.ProjectName.Worker
{
    [DependsOn(typeof(AbpBackgroundWorkersQuartzModule),
        typeof(AbpBackgroundWorkersModule))]
    public class BackgroundWorkersModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.AddBackgroundWorkerAsync<BackgroundWorker>();
        }
    }
}