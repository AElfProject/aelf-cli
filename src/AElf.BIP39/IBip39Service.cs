using AElf.BIP39.Types;

namespace AElf.BIP39
{
    public interface IBip39Service
    {
        Mnemonic GenerateMnemonic(int strength, BipWordlistLanguage language);
    }
}