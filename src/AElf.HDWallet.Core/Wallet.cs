using AElf.Cryptography;
using AElf.Cryptography.ECDSA;
using AElf.Types;
using NBitcoin;
using NBitcoin.DataEncoders;

namespace AElf.HDWallet.Core
{
    public abstract class Wallet : IWallet
    {
        public Key Key { get; set; }
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
                var hexEncodeData = Encoders.Hex.EncodeData(value);
                Key = PrivateKeyParse(hexEncodeData);
                _privateKey = value;
            }
        }

        private static Key PrivateKeyParse(string privateKey)
        {
            var privKeyPrefix = new byte[] {128};
            var prefixedPrivKey = Helper.Concat(privKeyPrefix, Encoders.Hex.DecodeData(privateKey));

            var privKeySuffix = new byte[] {1};
            var suffixedPrivKey = Helper.Concat(prefixedPrivKey, privKeySuffix);

            var base58Check = new Base58CheckEncoder();
            var privKeyEncoded = base58Check.EncodeData(suffixedPrivKey);
            return Key.Parse(privKeyEncoded, Network.Main);
        }

        public uint Index { get; set; }

        public ECKeyPair KeyPair { get; set; }

        protected abstract Address GenerateAddress();
    }
}