using AElf.BIP32;
using AElf.BIP39;
using AElf.HDWallet;
using AElf.Modularity;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace AElf.Wallet.Test
{
    [DependsOn(
        typeof(AbpTestBaseModule),
        typeof(AElfHDWalletModule),
        typeof(Bip39Module)
    )]
    public class AElfWalletTestModule : AElfModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}