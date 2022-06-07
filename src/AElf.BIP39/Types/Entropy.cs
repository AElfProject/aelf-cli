namespace AElf.BIP39.Types;

public class Entropy
{
    public Entropy()
    {
    }

    public Entropy(string hex, BipWordlistLanguage language = BipWordlistLanguage.English)
    {
        Hex = hex;
        Language = language;
    }

    public string Hex { get; set; }
    public BipWordlistLanguage Language { get; set; }

    public override string ToString()
    {
        return Hex;
    }
}