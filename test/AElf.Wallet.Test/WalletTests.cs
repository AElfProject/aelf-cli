using AElf.BIP39;
using AElf.BIP39.Types;
using AElf.Cryptography;
using AElf.HDWallet;
using AElf.HDWallet.Core;
using AElf.Types;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace AElf.Wallet.Test;

public sealed class WalletTests : AbpIntegratedTest<AElfWalletTestModule>
{
    private readonly IBip39Service _bip39Service;

    public WalletTests()
    {
        _bip39Service = GetRequiredService<IBip39Service>();
    }

    [Fact(Skip = "")]
    public void Test1()
    {
        const string mnemonicValue = "spawn spirit sure slender table actor ball health subject trade aware pilot";
        const string expectedPrivateKey = "923eed2ff8a0d246ca4d9387459bb1817c91ffcb4c137118a8ac46f86963009d";
        const string expectedPubkey =
            "0486ba629cdc52b2698f7ba3a6cda80efe121b7153054e3980696c57f31d15f07040d9de7665f9e99a24089b648275ed61fce6b75c7c29c1b7b819e3c2a3146b72";
        const string expectedAddress = "4HcpfWUnzyRx8k6f74ASWj94L92771ogcJ2YszMLW2VSfopdo";
        var mnemonic = new Mnemonic
        {
            Value = mnemonicValue,
            Language = BipWordlistLanguage.English
        };
        var seed = _bip39Service.ConvertMnemonicToSeedHex(mnemonic);
        var masterWallet = new AElfHDWallet(seed);
        var account = masterWallet.GetAccount(0);
        var wallet = account.GetExternalWallet(0);
        var keyPair = CryptoHelper.FromPrivateKey(wallet.PrivateKey);
        //keyPair.PublicKey.ToHex().ShouldBe(expectedPubkey);
        keyPair.PrivateKey.ToHex().ShouldBe(expectedPrivateKey);
        wallet.PrivateKey.ToHex().ShouldBe(expectedPrivateKey);
        wallet.Address.ToBase58().ShouldBe(expectedAddress);
        wallet.KeyPair.PublicKey.ToHex().ShouldBe(expectedPubkey);
    }

    [Fact]
    public void HDWalletTests()
    {
        var foo = Hash.LoadFromHex("faed1e53a2fc11baaa38aebc9a899a176fcf04288adf9cd3f3248e173b894e6b");
        var bar = foo.Value.ToBase64();
        const string mnemonicValue =
            "rapid apart clip require dragon property hurry ensure coil ship torch include squirrel jewel window";
        const string expectedSeed =
            "ba78b733ffe929e400f844751a48dded5ebc7c62635a1590e97b066e3b9e8b890741602a69279c45ed5d17dfd6e8703e3c575de4ea4712868df5f1997e2b97b2";
        const string expectedPubkey = "02ba33e9bba01836a3c5c8c4aa70abb16ccffc66470d40def867a0d66fa3d40c27";

        var mnemonic = new Mnemonic
        {
            Value = mnemonicValue,
            Language = BipWordlistLanguage.English
        };
        var seed = _bip39Service.ConvertMnemonicToSeedHex(mnemonic);
        seed.ShouldBe(expectedSeed);
        var masterWallet = new HDWallet<AElfWallet>(seed, "m/44'/60'");
        var account = masterWallet.GetAccount(0);
        var wallet = account.GetExternalWallet(0);
    }
}