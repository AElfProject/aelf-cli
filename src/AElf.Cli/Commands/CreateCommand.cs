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
    private readonly IBip39Service _bip39Service;
    private readonly IMnemonicService _mnemonicService;

    public CreateCommand(IBip39Service bip39Service, IMnemonicService mnemonicService)
    {
        _bip39Service = bip39Service;
        _mnemonicService = mnemonicService;
    }

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

        /*var mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
        var seed = mnemonic.DeriveSeed();
        var path = KeyPath.Parse("m/44'/1616'/0'/0/0");
        var extKey = ExtKey.CreateFromSeed(seed);
        var derived = extKey.Derive(path);
        var privateKey = derived.PrivateKey;
        var seedHex = _mnemonicService.ConvertMnemonicToSeedHex(mnemonic, "");*/

        //var mnemonic = _bip39Service.GenerateMnemonic(256, BipWordlistLanguage.English);
        //Logger.LogInformation(GetAccountInfo(mnemonic, ""));
    }

    public string GetUsageInfo()
    {
        return string.Empty;
    }

    public string GetShortDescription()
    {
        return string.Empty;
    }

    // private string GetAccountInfo(Mnemonic mnemonic, string password)
    // {
    //     var accountInfo = new StringBuilder();
    //     accountInfo.AppendLine($"[Mnemonic]{mnemonic}");
    //     var seedHex = _mnemonicService.ConvertMnemonicToSeedHex(mnemonic, password);
    //     return accountInfo.ToString();
    // }
}