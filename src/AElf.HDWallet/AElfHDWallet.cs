using AElf.HDWallet.Core;

namespace AElf.HDWallet;

public class AElfHDWallet : HDWallet<AElfWallet>
{
    public AElfHDWallet(string seed) : base(seed, AElfHDWalletConstants.AElfPath)
    {
    }
}