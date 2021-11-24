using AElf.Cryptography;
using AElf.Cryptography.ECDSA;
using AElf.Types;

namespace AElf.HDWallet.Core
{
    public abstract class Wallet : IWallet
    {
        public Address Address => GenerateAddress();

        public byte[] Sign(byte[] message)
        {
            return CryptoHelper.SignWithPrivateKey(_privateKey, message);
        }

        private byte[] _privateKey;

        public byte[] PrivateKey
        {
            get => _privateKey;
            set
            {
                _privateKey = value;
                KeyPair = CryptoHelper.FromPrivateKey(_privateKey);
            }
        }

        public uint Index { get; set; }

        public ECKeyPair KeyPair { get; set; }

        protected abstract Address GenerateAddress();
    }
}