using AElf.BIP39;
using AElf.HDWallet;
using AElf.HDWallet.Core;
using NBitcoin;
using Secp256k1Net;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;
using Mnemonic = AElf.BIP39.Types.Mnemonic;

namespace AElf.Wallet.Test
{
    public sealed class WalletTests : AbpIntegratedTest<AElfWalletTestModule>
    {
        private readonly IBip39Service _bip39Service;

        public WalletTests()
        {
            _bip39Service = GetRequiredService<IBip39Service>();
        }

        [Fact]
        public void Test1()
        {
            const string mnemonicValue = "spawn spirit sure slender table actor ball health subject trade aware pilot";
            const string expectedPubkey =
                "0486ba629cdc52b2698f7ba3a6cda80efe121b7153054e3980696c57f31d15f07040d9de7665f9e99a24089b648275ed61fce6b75c7c29c1b7b819e3c2a3146b72";
            var mnemonic = new Mnemonic
            {
                Value = mnemonicValue,
                Language = BipWordlistLanguage.English
            };
            var seed = _bip39Service.ConvertMnemonicToSeedHex(mnemonic);
            var wallet = new AElfHDWallet(seed).GetAccount(0).GetExternalWallet(0);
            wallet.Address.ToBase58().ShouldBe("4HcpfWUnzyRx8k6f74ASWj94L92771ogcJ2YszMLW2VSfopdo");
            wallet.KeyPair.PublicKey.ToHex().ShouldBe(expectedPubkey);
        }
    }
}