using System;
using System.Text;

namespace AElf.BIP39.Types
{
    public class Mnemonic
    {
        public string Value { get; set; }
        public BipWordlistLanguage Language { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}