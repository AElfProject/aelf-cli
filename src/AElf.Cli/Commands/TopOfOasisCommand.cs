using System;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Services;
using AElf.Types;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands
{
    public class TopOfOasisCommand: IAElfCommand, ITransientDependency
    {
        public const string Name = "oasis";

        private readonly ITopOfOasisService _topOfOasisService;
        private readonly IUserContext _userContext;
        private readonly IBlockChainService _blockChainService;
        
        public ILogger<TopOfOasisCommand> Logger { get; set; }

        public TopOfOasisCommand(ITopOfOasisService topOfOasisService, IUserContext userContext, IBlockChainService blockChainService)
        {
            _topOfOasisService = topOfOasisService;
            _userContext = userContext;
            _blockChainService = blockChainService;
            
            Logger = NullLogger<TopOfOasisCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var endpoint = _userContext.Endpoint;
            try
            {
                _userContext.Endpoint = AElfCliConstants.MainNetEndpoint;
                switch (commandLineArgs.Target?.ToLower())
                {
                    case "upload-project":
                        var txId = await _topOfOasisService.UploadProjectAsync();
                        var result = await _blockChainService.CheckTransactionResultAsync(txId);
                        if (result.Status == TransactionResultStatus.Mined.ToString().ToUpper())
                        {
                            var projectId = StringValue.Parser.ParseFrom(ByteArrayHelper.HexStringToByteArray(result.ReturnValue));
                            Logger.LogInformation($"Your project id is: {projectId.Value}.");
                        }
                        break;
                    default:
                        throw new AElfCliUsageException(
                            $"Oasis command: {commandLineArgs.Target} is not supported!" +
                            Environment.NewLine + Environment.NewLine +
                            GetUsageInfo());
                }
                
                
                await _topOfOasisService.UploadProjectAsync();
            }
            finally
            {
                _userContext.Endpoint = endpoint;
            }
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("Usage:");
            sb.AppendLine();
            sb.AppendLine("    aelf oasis <command>");
            sb.AppendLine();
            sb.AppendLine("    command: upload-project");
            sb.AppendLine();
            sb.AppendLine("Examples:");
            sb.AppendLine();
            sb.AppendLine("    aelf oasis upload-project");
            sb.AppendLine();
            sb.AppendLine("See the documentation for more info: https://docs.aelf.io");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "AElf Top Of Oasis.";
        }
    }
}