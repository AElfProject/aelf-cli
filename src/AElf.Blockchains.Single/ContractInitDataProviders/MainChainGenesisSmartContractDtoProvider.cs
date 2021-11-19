using System.Collections.Generic;
using AElf.Blockchains.BasicBaseChain;
using AElf.ContractDeployer;
using AElf.Kernel.SmartContract;
using AElf.Kernel.SmartContract.Application;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace AElf.Blockchains.Single
{
    public class MainChainGenesisSmartContractDtoProvider : GenesisSmartContractDtoProviderBase, ITransientDependency
    {
        private readonly ContractOptions _contractOptions;

        public MainChainGenesisSmartContractDtoProvider(IContractDeploymentListProvider contractDeploymentListProvider,
            IServiceContainer<IContractInitializationProvider> contractInitializationProviders,
            IOptionsSnapshot<ContractOptions> contractOptions)
            : base(contractDeploymentListProvider, contractInitializationProviders)
        {
            _contractOptions = contractOptions.Value;
        }

        protected override IReadOnlyDictionary<string, byte[]> GetContractCodes()
        {
            return ContractsDeployer.GetContractCodes<MainChainGenesisSmartContractDtoProvider>(_contractOptions
                .GenesisContractDir);
        }
    }
}
