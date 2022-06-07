using AElf.Modularity;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace AElf.BIP39.Test;

[DependsOn(
    typeof(AbpTestBaseModule),
    typeof(Bip39Module)
)]
public class Bip39TestModule : AElfModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}