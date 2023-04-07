using System.Text;
using System.Threading.Tasks;
using AElf.BIP39;
using AElf.Cli.Args;
using Microsoft.Extensions.Logging;
using NBitcoin;
using Nethereum.HdWallet;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands;

public class CreateCommand : IAElfCommand, ITransientDependency
{
    public const string Name = "create";
    public ILogger<CreateCommand> Logger { get; set; }
    
    public const string DEFAULT_PATH = "m/44'/1616'/0'/0/x";

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
        var wallet = new Wallet(mnemonic.ToString(),"",DEFAULT_PATH,null);
        var account = wallet.GetAccount(0);
        var address = wallet.GetAddresses()[0];
        var privateKey = wallet.GetPrivateKey(0);
        var publicKey = wallet.GetPublicKey(0);
        Logger.LogInformation("address is : {address}",address);
        Logger.LogInformation("privateKey is : {privateKey}",privateKey);
        Logger.LogInformation("publicKey is : {publicKey}",publicKey);

    }

    public string GetUsageInfo()
    {
        return string.Empty;
    }

    public string GetShortDescription()
    {
        return string.Empty;
    }


}