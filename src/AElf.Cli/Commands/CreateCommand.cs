using System.Text;
using System.Threading.Tasks;
using AElf.BIP39;
using AElf.BIP39.Types;
using AElf.Cli.Args;
using Microsoft.Extensions.Logging;
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

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var mnemonic = _bip39Service.GenerateMnemonic(256, BipWordlistLanguage.English);

        Logger.LogInformation(GetAccountInfo(mnemonic, ""));
    }

    public string GetUsageInfo()
    {
        return string.Empty;
    }

    public string GetShortDescription()
    {
        return string.Empty;
    }

    private string GetAccountInfo(Mnemonic mnemonic, string password)
    {
        var accountInfo = new StringBuilder();
        accountInfo.AppendLine($"[Mnemonic]{mnemonic}");
        var seedHex = _mnemonicService.ConvertMnemonicToSeedHex(mnemonic, password);
        return accountInfo.ToString();
    }
}