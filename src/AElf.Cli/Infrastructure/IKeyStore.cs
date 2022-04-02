using System.Collections.Generic;
using System.Threading.Tasks;
using AElf.Cryptography.ECDSA;

namespace AElf.Cli.Infrastructure
{
    public interface IKeyStore
    {
        Task<Cli.Infrastructure.AccountError> UnlockAccountAsync(string address, string password);

        ECKeyPair GetAccountKeyPair(string address);

        Task<ECKeyPair> CreateAccountKeyPairAsync(string password);

        Task<List<string>> GetAccountsAsync();
    }
}