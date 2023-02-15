using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AElf.Cli.Infrastructure;
using AElf.Cryptography;
using AElf.Cryptography.ECDSA;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services;

public interface IAccountsService
{
    void AddAccount(ECKeyPair keyPair);
    void RemoveAccount(string address);
    List<string> GetLocalAccount();
    Task<byte[]> SignAsync(string address, string password, byte[] data);
}

public class AccountsService : IAccountsService, ISingletonDependency
{
    private readonly IKeyStore _keyStore;

    public AccountsService(IKeyStore keyStore)
    {
        _keyStore = keyStore;
    }

    public void AddAccount(ECKeyPair keyPair)
    {
        // Add key file to aelf/keys
        throw new NotImplementedException();
    }

    public void RemoveAccount(string address)
    {
        // Remove key file from aelf/keys
        throw new NotImplementedException();
    }

    public List<string> GetLocalAccount()
    {
        // List addresses and pubkeys of files in aelf/keys
        throw new NotImplementedException();
    }

    public async Task<byte[]> SignAsync(string address, string password, byte[] data)
    {
        var signature =
            CryptoHelper.SignWithPrivateKey((await GetAccountKeyPairAsync(address, password)).PrivateKey, data);
        return signature;
    }

    private async Task<ECKeyPair> GetAccountKeyPairAsync(string address, string password)
    {
        var accountKeyPair = _keyStore.GetAccountKeyPair(address);
        if (accountKeyPair == null)
        {
            await _keyStore.UnlockAccountAsync(address, password);
            accountKeyPair = _keyStore.GetAccountKeyPair(address);
        }

        return accountKeyPair;
    }
}