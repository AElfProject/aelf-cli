using AElf.Cli.Services;

namespace AElf.Cli.TestBase.Generators;

public partial class Generate
{
    private readonly IBlockChainService _blockChainService;

    public Generate(IBlockChainService blockChainService)
    {
        _blockChainService = blockChainService;
    }

    public Generate A => new(_blockChainService);
    public Generate An => new(_blockChainService);
}