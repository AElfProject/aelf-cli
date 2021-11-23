using System;
using System.Linq;
using System.Security.Cryptography;

namespace AElf.BIP39.Extensions
{
    public static class ByteArrayExtensions
    {
        internal static string ToBinary(this byte[] bytes)
        {
            return string.Join("", bytes.Select(h => (Convert.ToString(h, 2).LeftPad("0", 8))));
        }

        internal static string GetChecksumBits(this byte[] checksum)
        {
            return new SHA256CryptoServiceProvider().ComputeHash(checksum).ToBinary()[..(checksum.Length * 8 / 32)];
        }
    }
}