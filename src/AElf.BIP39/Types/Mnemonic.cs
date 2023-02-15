namespace AElf.BIP39.Types;

public class Mnemonic
{
    public Mnemonic()
    {
    }

    public Mnemonic(string value, BipWordlistLanguage language = BipWordlistLanguage.English)
    {
        Value = value;
        Language = language;
    }

    public string Value { get; set; }
    public BipWordlistLanguage Language { get; set; }

    public override string ToString()
    {
        return Value;
    }
}