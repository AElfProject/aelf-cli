using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AElf.BIP32
{
    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class Bip32Module : AbpModule
    {
        
    }
}