using AElf.CrossChain;
using Volo.Abp.DependencyInjection;

namespace AElf.Blockchains.Single
{
    public class CrossChainContractInitializationDataProvider : ICrossChainContractInitializationDataProvider,
        ITransientDependency
    {
        public CrossChainContractInitializationData GetContractInitializationData()
        {
            return new CrossChainContractInitializationData
            {
                IsPrivilegePreserved = true
            };
        }
    }
}