using System.Collections.Generic;
using System.IO;
using AElf.Cli.Building.File;
using ICSharpCode.SharpZipLib.Zip;

namespace AElf.Cli.Building.Template.Step;

public class TemplateBuildStepCreateProjectResultZip:ITemplateBuildStep
{
    public void Execute(TemplateBuildContext context)
    {
        context.ProjectZipContent = CreateZipFileFromEntries(context.FileEntryList);
    }

    private static byte[] CreateZipFileFromEntries(IEnumerable<FileEntry> fileEntryList)
    {
        using var memoryStream = new MemoryStream();
        using (var zipOutputStream = new ZipOutputStream(memoryStream))
        {
            zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression

            foreach (var entry in fileEntryList)
            {
                zipOutputStream.PutNextEntry(new ZipEntry(entry.Name)
                {
                    Size = entry.Bytes.Length
                });
                zipOutputStream.Write(entry.Bytes, 0, entry.Bytes.Length);
            }

            zipOutputStream.CloseEntry();
            zipOutputStream.IsStreamOwner = false;
        }

        memoryStream.Position = 0;
        return memoryStream.ToArray();
    }
}
