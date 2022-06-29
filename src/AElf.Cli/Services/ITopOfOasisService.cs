using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services;

public interface ITopOfOasisService
{
    Task<string> UploadProjectAsync();
}

public class TopOfOasisService : ITopOfOasisService, ITransientDependency
{
    private readonly IBlockChainService _blockChainService;

    public TopOfOasisService(IBlockChainService blockChainService)
    {
        _blockChainService = blockChainService;
    }

    public async Task<string> UploadProjectAsync()
    {
        return await _blockChainService.SendTransactionAsync(AElfCliConstants.TopOfOasisContractAddress,
            "UploadProject");
    }
}