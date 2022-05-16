using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AElf.Cli.Args;
using AElf.Cli.Services;
using AElf.Contracts.MultiToken;
using AElf.Types;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Commands
{
    public class NFTCommand : IAElfCommand, ITransientDependency
    {
        public const string Name = "nft";

        private readonly IBlockChainService _blockChainService;

        public ILogger<NFTCommand> Logger { get; set; }

        public NFTCommand(IBlockChainService blockChainService)
        {
            _blockChainService = blockChainService;
            Logger = NullLogger<NFTCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var minter = commandLineArgs.Options.GetOrNull(Options.Minter.Short, Options.Minter.Long);
            var symbol = commandLineArgs.Options.GetOrNull(Options.Symbol.Short, Options.Symbol.Long);

            if (!string.IsNullOrEmpty(minter))
            {
                await AddMinter(symbol, minter);
                return;
            }

            var raw = await ValidateTokenInfoExistsAsync(symbol);
            
            var txId = commandLineArgs.Options.GetOrNull(Options.TxId.Short, Options.TxId.Long);

            var merklePath =
                await _blockChainService.GetMerklePathByTransactionIdAsync(txId,
                    AElfCliConstants.TestNetMainChainEndpoint);
            var txResultDto =
                await _blockChainService.GetTransactionResultAsync(txId, AElfCliConstants.TestNetMainChainEndpoint);
            var validateTokenInfoTxDto = txResultDto.Transaction;
            var validateTokenInfoTx = new Transaction
            {
                From = Address.FromBase58(validateTokenInfoTxDto.From),
                To = Address.FromBase58(validateTokenInfoTxDto.To),
                MethodName = validateTokenInfoTxDto.MethodName,
                Params = new ValidateTokenInfoExistsInput
                {
                    Symbol = symbol,
                    TokenName = "aelf基金会徽章",
                    TotalSupply = 10000,
                    Issuer = Address.FromBase58("2HeW7S9HZrbRJZeivMppUuUY3djhWdfVnP5zrDsz8wqq6hKMfT"),
                    IsBurnable = true,
                    IssueChainId = 1866392,
                    ExternalInfo =
                    {
                        ["aelf_nft_type"] = "Badges",
                        ["aelf_nft_token_id_reuse"] = "True",
                        ["aelf_nft_base_uri"] = ""
                    }
                }.ToByteString(),
                RefBlockNumber = validateTokenInfoTxDto.RefBlockNumber,
                RefBlockPrefix = ByteString.FromBase64(validateTokenInfoTxDto.RefBlockPrefix),
                Signature = ByteString.FromBase64(validateTokenInfoTxDto.Signature)
            };
            var crossChainCreateTokenInput = new CrossChainCreateTokenInput
            {
                FromChainId = AElfCliConstants.AElfChainId,
                ParentChainHeight = txResultDto.BlockNumber,
                TransactionBytes = validateTokenInfoTx.ToByteString(),
                MerklePath = new MerklePath
                {
                    MerklePathNodes =
                    {
                        merklePath.MerklePathNodes.Select(n => new MerklePathNode
                        {
                            Hash = Hash.LoadFromHex(n.Hash),
                            IsLeftChildNode = n.IsLeftChildNode
                        })
                    }
                }
            };
            var foo = new JArray();
            foreach (var merklePathNode in crossChainCreateTokenInput.MerklePath.MerklePathNodes)
            {
                foo.Add(new JObject
                {
                    ["hash"] = new JObject
                    {
                        ["value"] = merklePathNode.Hash.ToByteString().ToBase64()
                    },
                    ["isLeftChildNode"] = merklePathNode.IsLeftChildNode
                });
            }
            var @params = new JObject
            {
                ["fromChainId"] = crossChainCreateTokenInput.FromChainId,
                ["parentChainHeight"] = crossChainCreateTokenInput.ParentChainHeight,
                ["transactionBytes"] = validateTokenInfoTx.ToByteString().ToBase64(),
                ["merklePath"] = new JObject
                {
                    ["merklePathNodes"] = foo
                }
            };

            var result =
                await _blockChainService.SendTransactionAsync(AElfCliConstants.TestSideChainTokenContractAddress,
                    "CrossChainCreateToken", JsonConvert.SerializeObject(@params),
                    AElfCliConstants.TestNetSideChainEndpoint);

            await _blockChainService.CheckTransactionResultAsync(result, AElfCliConstants.TestNetSideChainEndpoint);
        }

        private async Task<string> ValidateTokenInfoExistsAsync(string symbol)
        {
            var tokenInfo = await _blockChainService.ExecuteTransactionAsync(AElfCliConstants.TestMainChainTokenContractAddress,
                "GetTokenInfo", JsonConvert.SerializeObject(new JObject
                {
                    ["symbol"] = symbol
                }), AElfCliConstants.TestNetMainChainEndpoint);
            
            var input = new JObject
            {
                ["symbol"] = symbol,
                ["tokenName"] = "aelf基金会徽章",
                ["totalSupply"] = 10000,
                ["issuer"] = new JObject
                {
                    ["value"] = Address.FromBase58("2HeW7S9HZrbRJZeivMppUuUY3djhWdfVnP5zrDsz8wqq6hKMfT").Value
                        .ToBase64()
                },
                ["isBurnable"] = true,
                ["issueChainId"] = 1866392,
                ["externalInfo"] = new JObject
                {
                    ["aelf_nft_type"] = "Badges",
                    ["aelf_nft_token_id_reuse"] = "True",
                    ["aelf_nft_base_uri"] = ""
                }
            };
            
            var (txId, rawTx) = await _blockChainService.SendTransactionReturnRawAsync(AElfCliConstants.TestMainChainTokenContractAddress,
                "ValidateTokenInfoExists", JsonConvert.SerializeObject(input), AElfCliConstants.TestNetMainChainEndpoint);
            var result = await _blockChainService.CheckTransactionResultAsync(txId, AElfCliConstants.TestNetMainChainEndpoint);
            Logger.LogInformation(result.Error);

            return rawTx;
        }
        

        private async Task AddMinter(string symbol, string address)
        {
            var minterListValue = new JObject
            {
                ["value"] = new JArray(new JObject
                {
                    ["value"] = Address.FromBase58(address).Value.ToBase64()
                })
            };
            var input = new JObject
            {
                ["symbol"] = symbol,
                ["minterList"] = minterListValue
            };
            await _blockChainService.SendTransactionAsync(AElfCliConstants.TestSideChainNFTContractAddress, "AddMinters",
                JsonConvert.SerializeObject(input), AElfCliConstants.TestNetSideChainEndpoint);
        }

        public string GetUsageInfo()
        {
            return string.Empty;
        }

        public string GetShortDescription()
        {
            return string.Empty;
        }

        public static class Options
        {
            public static class Symbol
            {
                public const string Short = "s";
                public const string Long = "symbol";
            }

            public static class TxId
            {
                public const string Short = "i";
                public const string Long = "txid";
            }

            public static class Minter
            {
                public const string Short = "m";
                public const string Long = "minter";
            }
        }
    }
}