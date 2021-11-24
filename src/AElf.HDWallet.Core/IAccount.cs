namespace AElf.HDWallet.Core
{
    public interface IAccount<out TWallet> where TWallet : IWallet, new()
    {
        TWallet GetInternalWallet(uint addressIndex);
        TWallet GetExternalWallet(uint addressIndex);
    }
}