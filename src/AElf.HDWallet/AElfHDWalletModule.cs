using AElf.HDWallet.Core;
using Volo.Abp.Modularity;

namespace AElf.HDWallet
{
    [DependsOn(
        typeof(AElfHDWalletCoreModule)
    )]
    public class AElfHDWalletModule : AbpModule
    {
        
    }
}