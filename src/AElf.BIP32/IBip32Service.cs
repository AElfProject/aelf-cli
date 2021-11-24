using AElf.BIP32.Types;
using AElf.Cryptography.ECDSA;

namespace AElf.BIP32
{
    public interface IBip32Service
    {
        ExtendedPrivateKey GetMasterXprvFromSeed(string seed);
        ExtendedPrivateKey DerivePath(string seed, string path = Bip32Constants.AElfPath);
        byte[] GetPublicKey(byte[] privateKey, bool withZeroByte = false);
        ECKeyPair GetECKeyPair(byte[] privateKey);
    }
}