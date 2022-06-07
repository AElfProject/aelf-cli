using System.IO;
using AElf.OS.Node.Infrastructure;

namespace AElf.Blockchains.Single;

public class CliNodeEnvironmentProvider : INodeEnvironmentProvider
{
    private const string ApplicationFolderName = "aelf";

    public string GetAppDataPath()
    {
        var type = typeof(CliNodeEnvironmentProvider);
        var currentDirectory = Path.GetDirectoryName(type.Assembly.Location);
        return Path.Combine(currentDirectory, ApplicationFolderName);
    }
}