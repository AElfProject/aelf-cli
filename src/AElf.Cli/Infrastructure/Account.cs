using AElf.Cryptography.ECDSA;

namespace AElf.Cli.Infrastructure;

public class Account
{
    public Account(string address)
    {
        AccountName = address;
    }

    public ECKeyPair KeyPair { get; set; }
    public string AccountName { get; }
}