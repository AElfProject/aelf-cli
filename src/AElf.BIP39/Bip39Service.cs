using System;
using System.Security.Cryptography;
using AElf.BIP39.Types;
using Volo.Abp.DependencyInjection;

namespace AElf.BIP39
{
    /// <summary>
    /// According to https://github.com/bitcoin/bips/blob/master/bip-0039.mediawiki
    /// </summary>
    public class Bip39Service : IBip39Service, ITransientDependency
    {
        private readonly IEntropyService _entropyService;
        private readonly IMnemonicService _mnemonicService;

        public Bip39Service(IEntropyService entropyService, IMnemonicService mnemonicService)
        {
            _entropyService = entropyService;
            _mnemonicService = mnemonicService;
        }

        public Mnemonic GenerateMnemonic(int strength, BipWordlistLanguage language)
        {
            if (strength % 32 != 0)
            {
                throw new NotSupportedException(Bip39Constants.InvalidEntropy);
            }

            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var buffer = new byte[strength / 8];
            rngCryptoServiceProvider.GetBytes(buffer);
            var entropy = new Entropy(BitConverter.ToString(buffer).Replace("-", ""), language);
            return ConvertEntropyToMnemonic(entropy);
        }

        public Mnemonic ConvertEntropyToMnemonic(Entropy entropy)
        {
            return _entropyService.ConvertEntropyToMnemonic(entropy);
        }

        public Entropy ConvertMnemonicToEntropy(Mnemonic mnemonic)
        {
            return _mnemonicService.ConvertMnemonicToEntropy(mnemonic);
        }

        public string ConvertMnemonicToSeedHex(Mnemonic mnemonic, string password = null)
        {
            return _mnemonicService.ConvertMnemonicToSeedHex(mnemonic, password);
        }

        public bool ValidateMnemonic(Mnemonic mnemonic)
        {
            return _mnemonicService.ValidateMnemonic(mnemonic);
        }
    }
}