using AElf.Modularity;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace AElf.BIP32.Test
{
    [DependsOn(
        typeof(AbpTestBaseModule),
        typeof(Bip32Module)
    )]
    public class Bip32TestModule : AElfModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}