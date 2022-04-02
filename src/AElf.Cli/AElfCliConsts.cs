using System;
using System.IO;

namespace AElf.Cli
{
    public class AElfCliConsts
    {
        public const string EndpointConfigKey = "endpoint";
        public const string AccountConfigKey = "account";
        public const string PasswordConfigKey = "password";
        public const string AElfNativeSymbol = "ELF";
        public const string TestNetEndpoint = "http://18.163.40.216:8000";
        public const string MainNetEndpoint = "http://18.185.93.36:8000";
        public const string FaucetContractAddress = "2M24EKAecggCnttZ9DUUMCXi4xC67rozA87kFgid9qEwRUMHTs";
        
        // TODO: Update the contract address after it is deployed on the main net
        public const string TopOfOasisContractAddress = "2NxwCPAGJr4knVdmwhb1cK7CkZw5sMJkRDLnT7E2GoDP2dy5iZ"; 

        public static string DataPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "aelf");
    }
}