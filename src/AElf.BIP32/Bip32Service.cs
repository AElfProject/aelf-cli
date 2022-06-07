using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AElf.BIP32.Extensions;
using AElf.BIP32.Types;
using AElf.Cryptography;
using AElf.Cryptography.ECDSA;
using Chaos.NaCl;
using Volo.Abp.DependencyInjection;

namespace AElf.BIP32;

public class Bip32Service : IBip32Service, ITransientDependency
{
    public ExtendedPrivateKey GetMasterXprvFromSeed(string seed)
    {
        using var hmacSha512 = new HMACSHA512(Encoding.UTF8.GetBytes(Bip32Constants.Curve));
        var hash = hmacSha512.ComputeHash(seed.HexToByteArray());

        var left = hash.Slice(0, 32);
        var right = hash.Slice(32);

        return new ExtendedPrivateKey
        {
            PrivateKey = left,
            ChainCode = right
        };
    }

    public ExtendedPrivateKey DerivePath(string seed, string path = Bip32Constants.AElfPath)
    {
        if (!IsValidPath(path))
            throw new FormatException("Invalid derivation path.");

        var masterKey = GetMasterXprvFromSeed(seed);

        var segments = path
            .Split('/')
            .Slice(1)
            .Select(a => a.Replace("'", string.Empty))
            .Select(a => Convert.ToUInt32(a, 10));

        var results = segments
            .Aggregate(masterKey, (mks, next) => ChildKeyDerivation(mks, next + Bip32Constants.HardenedOffset));

        return results;
    }

    public byte[] GetPublicKey(byte[] privateKey, bool withZeroByte = false)
    {
        Ed25519.KeyPairFromSeed(out var publicKey, out _, privateKey);

        var zero = new byte[] { 0 };

        var buffer = new BigEndianBuffer();
        if (withZeroByte)
            buffer.Write(zero);

        buffer.Write(publicKey);

        return buffer.ToArray();
    }

    public ECKeyPair GetECKeyPair(byte[] privateKey)
    {
        return CryptoHelper.FromPrivateKey(privateKey);
    }

    private ExtendedPrivateKey ChildKeyDerivation(ExtendedPrivateKey xprv, uint index)
    {
        var buffer = new BigEndianBuffer();

        buffer.Write(new byte[] { 0 });
        buffer.Write(xprv.PrivateKey);
        buffer.WriteUInt(index);

        using var hmacSha512 = new HMACSHA512(xprv.ChainCode);
        var hash = hmacSha512.ComputeHash(buffer.ToArray());

        var left = hash.Slice(0, 32);
        var right = hash.Slice(32);

        return new ExtendedPrivateKey
        {
            PrivateKey = left,
            ChainCode = right
        };
    }

    private bool IsValidPath(string path)
    {
        var regex = new Regex("^m(\\/[0-9]+')+$");

        if (!regex.IsMatch(path))
            return false;

        var valid = !path.Split('/')
            .Slice(1)
            .Select(a => a.Replace("'", string.Empty))
            .Any(a => !uint.TryParse(a, out _));

        return valid;
    }
}