using AElf.HDWallet.Core;
using AElf.Types;

namespace AElf.HDWallet;

public class AElfWallet : Wallet
{
    protected override Address GenerateAddress()
    {
        return Address.FromPublicKey(KeyPair.PublicKey);
    }
}