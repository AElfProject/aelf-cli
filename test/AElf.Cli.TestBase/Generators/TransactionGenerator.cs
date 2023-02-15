using AElf.Cli.Services;
using AElf.Client.Dto;
using Volo.Abp.Threading;

namespace AElf.Cli.TestBase.Generators;

public class TransactionGenerator<T> : GeneratorBase<T> where T : TransactionDto, new()
{
    private readonly IBlockChainService _blockchainService;

    public TransactionGenerator(IBlockChainService blockchainService)
    {
        _blockchainService = blockchainService;
        TestObjectInternal = new T();
    }

    public TransactionGenerator<T> WithFrom(string from)
    {
        TestObjectInternal.From = from;
        return this;
    }

    public TransactionGenerator<T> WithTo(string to)
    {
        TestObjectInternal.To = to;
        return this;
    }

    public TransactionGenerator<T> WithContractName(string contractName)
    {
        TestObjectInternal.To =
            AsyncHelper.RunSync(() => _blockchainService.GetContractAddressByNameAsync(contractName));
        return this;
    }

    public TransactionGenerator<T> WithParams(string @params)
    {
        TestObjectInternal.Params = @params;
        return this;
    }

    public TransactionGenerator<T> WithMethodName(string methodName)
    {
        TestObjectInternal.MethodName = methodName;
        return this;
    }
}