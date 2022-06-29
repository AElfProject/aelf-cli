using AElf.BIP39;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AElf.HDWallet.Core;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(Bip39Module)
)]
public class AElfHDWalletCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
    }
}