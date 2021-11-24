using System;
using System.Linq;

namespace AElf.BIP32.Extensions
{
    public static class StringExtensions
    {
        public static byte[] HexToByteArray(this string hex)
        {
            var bytes = Enumerable.Range(0, hex.Length / 2)
                .Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16))
                .ToArray();

            return bytes;
        }
    }
}