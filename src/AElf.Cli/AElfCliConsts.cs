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

        public static string DataPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "aelf");
    }
}