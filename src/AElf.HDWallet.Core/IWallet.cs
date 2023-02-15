using AElf.Types;

namespace AElf.HDWallet.Core;

public interface IWallet
{
    Address Address { get; }

    public byte[] PrivateKey { get; set; }

    public uint Index { get; set; }

    byte[] Sign(byte[] message);
}