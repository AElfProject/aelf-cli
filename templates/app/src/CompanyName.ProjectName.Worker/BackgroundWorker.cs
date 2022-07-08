using Microsoft.Extensions.Logging;
using Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;

namespace CompanyName.ProjectName.Worker
{
    public class BackgroundWorker: QuartzBackgroundWorkerBase
    {
        //private readonly IService _service;
        
        public BackgroundWorker( 
            /* 
            IService service
            */)
        {
            //_service = service;
            JobDetail = JobBuilder.Create<BackgroundWorker>().WithIdentity(nameof(BackgroundWorker)).Build();
            Trigger = TriggerBuilder
                .Create()
                .WithIdentity(nameof(BackgroundWorker))
                .WithCronSchedule("0 0/5 * * * ?") 
                .StartNow()
                .Build();
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            Logger.LogInformation("Before Worker.");

            //Do something what you want.
            //await _service.DoSomething();
            
            Logger.LogInformation("After Worker.");
        }
    }
}