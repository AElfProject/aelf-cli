using System;
using System.IO;

namespace AElf.Cli
{
    public class AElfCliConsts
    {
        public const string EndpointConfigKey = "endpoint";
        public const string AddressConfigKey = "address";
        public const string PasswordConfigKey = "password";

        public static string DataPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "aelf");
    }
}