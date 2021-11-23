using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AElf.BIP39
{
    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class Bip39Module : AbpModule
    {
        
    }
}