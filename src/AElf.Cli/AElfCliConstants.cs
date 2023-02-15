using System;
using System.IO;

namespace AElf.Cli;

public class AElfCliConstants
{
    public const int AElfChainId = 9992731;
    public const string EndpointConfigKey = "endpoint";
    public const string AccountConfigKey = "account";
    public const string PasswordConfigKey = "password";
    public const string AElfNativeSymbol = "ELF";
    public const string TestNetMainChainEndpoint = "https://aelf-test-node.aelf.io";
    public const string TestNetSideChainEndpoint = "https://tdvv-test-node.aelf.io";
    public const string MainNetEndpoint = "http://18.185.93.36:8000";
    public const string FaucetContractAddress = "2M24EKAecggCnttZ9DUUMCXi4xC67rozA87kFgid9qEwRUMHTs";

    // TODO: Update the contract address after it is deployed on the main net
    public const string TopOfOasisContractAddress = "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ";

    public const string TestMainChainTokenContractAddress = "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE";
    public const string TestMainChainNFTContractAddress = "2VTusxv6BN4SQDroitnWyLyQHWiwEhdWU76PPiGBqt5VbyF27J";

    public const string TestSideChainTokenContractAddress = "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX";
    public const string TestSideChainNFTContractAddress = "2nkBVPGWcQLv1HLHpjLpwCrUNh7oSbzFbMgFnwUcM6tDXivRBw";
    public const string TestSideChainForestContractAddress = "2LhQEonazAugHnS6VDZmGjcnsGcgD8RgSF9WKxYUvAgQTB8oSS";

    public static string DataPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "aelf");
}