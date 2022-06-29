namespace AElf.BIP39;

public interface IBipWordlistProvider
{
    string[] LoadWordlist(BipWordlistLanguage language);
}