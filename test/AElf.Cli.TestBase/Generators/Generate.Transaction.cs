using AElf.Client.Dto;

namespace AElf.Cli.TestBase.Generators;

public partial class Generate
{
    public TransactionGenerator<TransactionDto> Transaction => new(_blockChainService);
}