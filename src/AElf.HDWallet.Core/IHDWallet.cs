namespace AElf.HDWallet.Core
{
    public interface IHDWallet<out TWallet> where TWallet : IWallet, new()
    {
        TWallet GetMasterWallet();
        TWallet GetAccountWallet(uint accountIndex);
        IAccount<TWallet> GetAccount(uint index);
    }
}