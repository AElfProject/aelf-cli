using System;
using System.Linq;
using AElf.Blockchains.BasicBaseChain;
using AElf.Database;
using AElf.Kernel.Infrastructure;
using AElf.Kernel.SmartContract;
using AElf.Kernel.SmartContract.Application;
using AElf.Modularity;
using AElf.OS.Node.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Modularity;

namespace AElf.Blockchains.Single
{
    [DependsOn(
        typeof(BasicBaseChainAElfModule)
    )]
    public class SingleNodeChainAElfModule : AElfModule
    {
        public ILogger<SingleNodeChainAElfModule> Logger { get; set; }

        public SingleNodeChainAElfModule()
        {
            Logger = NullLogger<SingleNodeChainAElfModule>.Instance;
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddTransient<IContractDeploymentListProvider, SingleNodeChainContractDeploymentListProvider>();

            services.AddKeyValueDbContext<BlockchainKeyValueDbContext>(p => p.UseInMemoryDatabase());
            services.AddKeyValueDbContext<StateKeyValueDbContext>(p => p.UseInMemoryDatabase());

            Configure<ContractOptions>(o => o.ContractDeploymentAuthorityRequired = false);

            services.AddSingleton<INodeEnvironmentProvider, CliNodeEnvironmentProvider>();
        }
    }
}