using NBitcoin;

namespace AElf.HDWallet.Core
{
    public class HDWallet<TWallet> : IHDWallet<TWallet> where TWallet : IWallet, new()
    {
        public string Seed { get; set; }

        private readonly ExtKey _masterKey;

        public HDWallet(string seed, string coinPath)
        {
            Seed = seed;
            var masterKeyPath = new KeyPath(coinPath);
            _masterKey = new ExtKey(seed).Derive(masterKeyPath);
        }

        public TWallet GetMasterWallet()
        {
            return new TWallet
            {
                PrivateKey = new ExtKey(Seed).PrivateKey.ToBytes()
            };
        }

        public TWallet GetAccountWallet(uint accountIndex)
        {
            var externalKeyPath = new KeyPath($"{accountIndex}'");
            _masterKey.Derive(externalKeyPath);

            return new TWallet
            {
                PrivateKey = _masterKey.PrivateKey.ToBytes(),
                Index = accountIndex
            };
        }

        public IAccount<TWallet> GetAccount(uint index)
        {
            var externalKeyPath = new KeyPath($"{index}'/0");
            var internalKeyPath = new KeyPath($"{index}'/1");
            return new Account<TWallet>(index, _masterKey.Derive(externalKeyPath), _masterKey.Derive(internalKeyPath));
        }
    }
}