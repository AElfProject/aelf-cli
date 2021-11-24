using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AElf.BIP39;
using AElf.BIP39.Types;
using AElf.Cli.Args;
using AElf.Cryptography;
using Microsoft.Extensions.Logging;

namespace AElf.Cli.Commands
{
    public class CreateCommand : IAElfCommand
    {
        private readonly IBip39Service _bip39Service;
        private readonly IMnemonicService _mnemonicService;
        public const string Name = "create";

        public ILogger<CreateCommand> Logger { get; set; }

        public CreateCommand(IBip39Service bip39Service, IMnemonicService mnemonicService)
        {
            _bip39Service = bip39Service;
            _mnemonicService = mnemonicService;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var mnemonic = _bip39Service.GenerateMnemonic(256, BipWordlistLanguage.English);

            Logger.LogInformation(GetAccountInfo(mnemonic, ""));
        }

        private string GetAccountInfo(Mnemonic mnemonic, string password)
        {
            var accountInfo = new StringBuilder();
            accountInfo.AppendLine($"[Mnemonic]{mnemonic}");
            var seedHex = _mnemonicService.ConvertMnemonicToSeedHex(mnemonic, password);
            return accountInfo.ToString();
        }

        public string GetUsageInfo()
        {
            throw new System.NotImplementedException();
        }

        public string GetShortDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}