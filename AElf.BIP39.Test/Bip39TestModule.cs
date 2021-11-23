using AElf.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace AElf.BIP39.Test
{
    [DependsOn(
        typeof(AbpTestBaseModule),
        typeof(Bip39Module)
    )]
    public class Bip39TestModule : AElfModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddTransient<IEntropyService, EntropyService>();
            services.AddTransient<IMnemonicService, MnemonicService>();
            services.AddTransient<IBip39Service, Bip39Service>();
        }
    }
}