using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AElf.BIP39.Extensions;
using AElf.BIP39.Types;
using Volo.Abp.DependencyInjection;

namespace AElf.BIP39
{
    public interface IMnemonicService
    {
        Entropy ConvertMnemonicToEntropy(Mnemonic mnemonic);
        string ConvertMnemonicToSeedHex(Mnemonic mnemonic, string password);
        bool ValidateMnemonic(Mnemonic mnemonic);
    }

    public class MnemonicService : IMnemonicService, ITransientDependency
    {
        private readonly IBipWordlistProvider _wordlistProvider;

        public MnemonicService(IBipWordlistProvider wordlistProvider)
        {
            _wordlistProvider = wordlistProvider;
        }

        public Entropy ConvertMnemonicToEntropy(Mnemonic mnemonic)
        {
            var wordlist = _wordlistProvider.LoadWordlist(mnemonic.Language);
            var words = mnemonic.Value.Normalize(NormalizationForm.FormKD).Split(new[] {' '},
                StringSplitOptions.RemoveEmptyEntries);

            if (words.Length % 3 != 0)
            {
                throw new FormatException(Bip39Constants.InvalidMnemonic);
            }

            var bits = string.Join("", words.Select(word =>
            {
                var index = Array.IndexOf(wordlist, word);
                if (index == -1)
                {
                    throw new FormatException(Bip39Constants.InvalidMnemonic);
                }

                return Convert.ToString(index, 2).LeftPad("0", 11);
            }));

            var dividerIndex = (int) Math.Floor((double) bits.Length / 33) * 32;
            var entropyBits = bits.Substring(0, dividerIndex);
            var checksumBits = bits.Substring(dividerIndex);

            var entropyBytesMatch = Regex.Matches(entropyBits, "(.{1,8})")
                .Select(m => m.Groups[0].Value)
                .ToArray();

            var entropyBytes = entropyBytesMatch
                .Select(bytes => Convert.ToByte(bytes, 2)).ToArray();

            var newChecksum = entropyBytes.GetChecksumBits();

            if (newChecksum != checksumBits)
                throw new Exception(Bip39Constants.InvalidChecksum);

            return new Entropy
            {
                Hex = BitConverter
                    .ToString(entropyBytes)
                    .Replace("-", "")
                    .ToLower(),
                Language = mnemonic.Language
            };
        }

        public string ConvertMnemonicToSeedHex(Mnemonic mnemonic, string password)
        {
            var mnemonicBytes = Encoding.UTF8.GetBytes(mnemonic.Value.Normalize(NormalizationForm.FormKD));
            var saltSuffix = password.IsNullOrEmpty() ? string.Empty : password;
            var salt = $"mnemonic{saltSuffix}";
            var saltBytes = Encoding.UTF8.GetBytes(salt);

            var rfc2898DerivedBytes = new Rfc2898DeriveBytes(mnemonicBytes, saltBytes, 2048, HashAlgorithmName.SHA512);
            var key = rfc2898DerivedBytes.GetBytes(64);
            var hex = BitConverter
                .ToString(key)
                .Replace("-", "")
                .ToLower();

            return hex;
        }

        public bool ValidateMnemonic(Mnemonic mnemonic)
        {
            try
            {
                ConvertMnemonicToEntropy(mnemonic);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}