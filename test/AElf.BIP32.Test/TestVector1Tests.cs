using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace AElf.BIP32.Test;

/// <summary>
///     https://github.com/bitcoin/bips/blob/master/bip-0032.mediawiki#test-vectors
/// </summary>
public sealed class Bip32Tests : AbpIntegratedTest<Bip32TestModule>
{
    private const string Seed = "000102030405060708090a0b0c0d0e0f";
    private const string Vector1KeyHexExpected = "2b4be7f19ee27bbf30c667b642d5f4aa69fd169872f8fc3059c08ebae2eb19e7";

    private const string Vector1ChainCodeExpected =
        "90046a93de5380a72b5e45010748567d5ea02bbf6522f979e05c0d8d8ca9fffb";

    private readonly IBip32Service _bip32Service;

    public Bip32Tests()
    {
        _bip32Service = GetRequiredService<IBip32Service>();
    }

    public void Test1()
    {
        const string expectedPath = "m/0'";
        const string expectedChainCode = "8b59aa11380b624e81507a27fedda59fea6d0b779a778918a2fd3590e16e9c69";
        const string expectedPrivateKey = "68e0fe46dfb67e368c75379acec591dad19df3cde26e63b93a8e704f1dade7a3";
        const string expectedPublicKey = "008c8a13df77a28f3445213a0f432fde644acaa215fc72dcdf300d5efaa85d350c";

        var masterXprv = _bip32Service.GetMasterXprvFromSeed(Seed);
        masterXprv.PrivateKey.ToHex().ShouldBe(Vector1KeyHexExpected);
        masterXprv.ChainCode.ToHex().ShouldBe(Vector1ChainCodeExpected);

        var xprv = _bip32Service.DerivePath(Seed, expectedPath);
        xprv.PrivateKey.ToHex().ShouldBe(expectedPrivateKey);
        xprv.ChainCode.ToHex().ShouldBe(expectedChainCode);

        var publicKey = _bip32Service.GetPublicKey(xprv.PrivateKey, true);
        publicKey.ToHex().ShouldBe(expectedPublicKey);
    }
}