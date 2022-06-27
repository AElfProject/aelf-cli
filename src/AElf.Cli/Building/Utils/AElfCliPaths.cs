using System;
using System.IO;

namespace AElf.Cli.Building.Utils;

public static class AElfCliPaths
{
    private static readonly string AElfRootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".aelf");
    
    public static string TemplateCache => Path.Combine(AElfRootPath, "templates");

    public static string TemplateCachedLocation(string templateName)
    {
        return Path.Join(TemplateCache,templateName);
    }

    public static string TemplateCachedFilename(string cachedLocation, string version)
    {
        return Path.Join(cachedLocation, version + ".zip");
    }
}