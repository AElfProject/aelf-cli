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

        public Bip39Service(IEntropyService entropyService)
        {
            _entropyService = entropyService;
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

            var entropy = new Entropy
            {
                Hex = BitConverter.ToString(buffer).Replace("-", ""),
                Language = language
            };

            return _entropyService.ConvertEntropyToMnemonic(entropy);
        }
    }
}