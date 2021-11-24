namespace AElf.BIP32
{
    public class Bip32Constants
    {
        public const string Curve = "ed25519 seed";
        public const uint HardenedOffset = 0x80000000;
        /// <summary>
        /// m / purpose' / coin_type' / account' / change / address_index
        /// purpose: always 44
        /// coin_type: AELF is 1616 (https://github.com/satoshilabs/slips/blob/master/slip-0044.md)
        /// 
        /// </summary>
        public const string AElfPath = @"m/44'/1616'/0'/0'/0'";
    }
}