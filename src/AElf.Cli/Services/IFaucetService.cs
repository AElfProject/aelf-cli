using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services
{
    public interface IFaucetService
    {
        Task<string> TaskAsync(string symbol, long amount);
    }

    public class FaucetService : IFaucetService, ITransientDependency
    {
        private readonly IBlockChainService _blockChainService;

        public FaucetService(IBlockChainService blockChainService)
        {
            _blockChainService = blockChainService;
        }

        public async Task<string> TaskAsync(string symbol, long amount)
        {
            var @params = new JObject();
            @params["symbol"] = symbol;
            @params["amount"] = amount;

            return await _blockChainService.SendTransactionAsync(AElfCliConsts.FaucetContractAddress, "Take", JsonConvert.SerializeObject(@params));
        }
    }
}