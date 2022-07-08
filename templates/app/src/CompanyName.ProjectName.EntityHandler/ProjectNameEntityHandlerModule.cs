using CompanyName.ProjectName.EntityFrameworkCore;
using CompanyName.ProjectName.EntityHandler.Core;
using CompanyName.ProjectName.Worker;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;
using Microsoft.Extensions.Configuration;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Aliyun;

namespace CompanyName.ProjectName.EntityHandler
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(ProjectNameEntityFrameworkCoreModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(ProjectNameEntityHandlerCoreModule),
        typeof(AbpBlobStoringAliyunModule),
        typeof(BackgroundWorkersModule)
    )]
    public class ProjectNameEntityHandlerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            ConfigureCache(configuration);
            ConfigureRedis(context, configuration);
            ConfigureBlob(configuration);
            context.Services.AddHostedService<ProjectNameHostedService>();
        }
        
        private void ConfigureBlob(IConfiguration configuration)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseAliyun(_ =>
                    {
                    });
                });
            });
        }

        private void ConfigureCache(IConfiguration configuration)
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "ProjectName:";
            });
        }

        private void ConfigureRedis(
            ServiceConfigurationContext context,
            IConfiguration configuration)
        {
            var config = configuration["Redis:Configuration"];
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            context.Services
                .AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "ProjectName-Protection-Keys");
        }
    }
}