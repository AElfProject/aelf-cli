using System.Collections.Generic;
using AElf.Cryptography.ECDSA;
using AElf.Types;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services
{
    public interface IAccountsService
    {
        void AddAccount(ECKeyPair keyPair);
        void RemoveAccount(string address);
        List<string> GetLocalAccount();
    }

    public class AccountsService : IAccountsService, ISingletonDependency
    {
        public void AddAccount(ECKeyPair keyPair)
        {
            // Add key file to aelf/keys
            throw new System.NotImplementedException();
        }

        public void RemoveAccount(string address)
        {
            // Remove key file from aelf/keys
            throw new System.NotImplementedException();
        }

        public List<string> GetLocalAccount()
        {
            // List addresses and pubkeys of files in aelf/keys
            throw new System.NotImplementedException();
        }
    }
}