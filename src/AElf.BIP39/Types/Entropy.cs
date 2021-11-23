namespace AElf.BIP39.Types
{
    public class Entropy
    {
        public string Hex { get; set; }
        public BipWordlistLanguage Language { get; set; }

        public override string ToString()
        {
            return Hex;
        }
    }
}