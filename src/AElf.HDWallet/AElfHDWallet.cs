namespace AElf.HDWallet
{
    public class AElfHDWallet : Core.HDWallet<AElfWallet>
    {
        public AElfHDWallet(string seed) : base(seed, AElfHDWalletConstants.AElfPath)
        {

        }
    }
}