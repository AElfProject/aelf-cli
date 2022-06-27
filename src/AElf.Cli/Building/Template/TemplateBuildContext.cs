using System.Collections.Generic;
using AElf.Cli.Building.File;
using AElf.Cli.Building.Utils;
using AElf.Cli.Commands;

namespace AElf.Cli.Building.Template;

public class TemplateBuildContext
{
    public static string OriginalProjectName => "AElf.Common";
    
    public string DownloadUrl { get; }

    public string CachedLocation { get; }
    
    public string CachedFileName { get; }
    
    public IList<FileEntry> FileEntryList { get; set; }
    
    public byte[] TemplateZipContent { get; set; }
    
    public byte[] ProjectZipContent { get; set; }

    public TemplateBuildContext(NewCommand.NewCommandArgs args)
    {
        CachedLocation = AElfCliPaths.TemplateCachedLocation(args.TemplateName);
        CachedFileName = AElfCliPaths.TemplateCachedFilename(CachedLocation, args.Version);
        DownloadUrl = AElfCliHttpAddresses.TemplateDownloadUrl(args.TemplateName, args.Version);
    }
}