namespace AElf.BIP32.Types
{
    /// <summary>
    /// xprv
    /// </summary>
    public class ExtendedPrivateKey
    {
        public byte[] PrivateKey { get; set; }
        public byte[] ChainCode { get; set; }

        public override string ToString()
        {
            return $"xprv{PrivateKey.ToPlainBase58()}{ChainCode.ToPlainBase58()}";
        }
    }
}