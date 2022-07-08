using AElf.AElfNode.EventHandler.BackgroundJob.Options;
using CompanyName.ProjectName.ContractEventHandler.Core;
using CompanyName.ProjectName.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;

namespace CompanyName.ProjectName.ContractEventHandler
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(ProjectNameEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(ContractEventHandlerCoreModule)
    )]
    public class ContractEventHandlerModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            ConfigureCache(configuration);
            ConfigureRedis(context, configuration);
            
            context.Services.AddHostedService<HostedService>();
            
            Configure<AElfProcessorOption>(options =>
            {
                configuration.GetSection("AElfEventProcessors").Bind(options);
            });
        }

        private void ConfigureCache(IConfiguration configuration)
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "KeyPrefix:";
            });
        }

        private void ConfigureRedis(
            ServiceConfigurationContext context,
            IConfiguration configuration)
        {
            var config = configuration["Redis:Configuration"];
            if (string.IsNullOrEmpty(config))
            {
                return;
            }

            var redis = ConnectionMultiplexer.Connect(config);
            context.Services
                .AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "Protection-Keys");
        }
    }
}