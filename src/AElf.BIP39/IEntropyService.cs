using System;
using System.Linq;
using System.Text.RegularExpressions;
using AElf.BIP39.Extensions;
using AElf.BIP39.Types;
using Volo.Abp.DependencyInjection;

namespace AElf.BIP39
{
    public interface IEntropyService
    {
        Mnemonic ConvertEntropyToMnemonic(Entropy entropy);
    }

    internal class EntropyService : IEntropyService, ITransientDependency
    {
        private readonly IBipWordlistProvider _wordlistProvider;

        public EntropyService(IBipWordlistProvider wordlistProvider)
        {
            _wordlistProvider = wordlistProvider;
        }

        public Mnemonic ConvertEntropyToMnemonic(Entropy entropy)
        {
            var wordlist = _wordlistProvider.LoadWordlist(entropy.Language);

            var entropyBytes = Enumerable.Range(0, entropy.Hex.Length / 2)
                .Select(x => Convert.ToByte(entropy.Hex.Substring(x * 2, 2), 16))
                .ToArray();
            var entropyBits = entropyBytes.ToBinary();
            var checksumBits = entropyBytes.GetChecksumBits();
            var bits = $"{entropyBits}{checksumBits}";
            var chunks = Regex.Matches(bits, "(.{1,11})")
                .Select(m => m.Groups[0].Value)
                .ToArray();

            var words = chunks.Select(binary =>
            {
                var index = Convert.ToInt32(binary, 2);
                return wordlist[index];
            });

            var joinedText = string.Join(entropy.Language == BipWordlistLanguage.Japanese ? "\u3000" : " ", words);

            return new Mnemonic(joinedText, entropy.Language);
        }
    }
}