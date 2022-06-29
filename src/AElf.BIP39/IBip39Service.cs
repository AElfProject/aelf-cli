using AElf.BIP39.Types;

namespace AElf.BIP39;

public interface IBip39Service
{
    Mnemonic GenerateMnemonic(int strength, BipWordlistLanguage language);
    Mnemonic ConvertEntropyToMnemonic(Entropy entropy);
    Entropy ConvertMnemonicToEntropy(Mnemonic mnemonic);
    string ConvertMnemonicToSeedHex(Mnemonic mnemonic, string password = null);
    bool ValidateMnemonic(Mnemonic mnemonic);
}