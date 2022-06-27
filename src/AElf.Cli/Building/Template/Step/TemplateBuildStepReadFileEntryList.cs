using System;
using System.IO;
using System.Collections.Generic;
using AElf.Cli.Building.File;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace AElf.Cli.Building.Template.Step;

public class TemplateBuildStepReadFileEntryList:ITemplateBuildStep
{
    public void Execute(TemplateBuildContext context)
    {
        context.FileEntryList = GetEntriesFromZipFile(context.TemplateZipContent);
    }

    private static IList<FileEntry> GetEntriesFromZipFile(byte[] fileBytes)
    {
        var fileEntries = new List<FileEntry>();

        using var memoryStream = new MemoryStream(fileBytes);
        using var zipInputStream = new ZipInputStream(memoryStream);
        var zipEntry = zipInputStream.GetNextEntry();
        while (zipEntry != null)
        {
            var buffer = new byte[4096]; // 4K is optimum

            using (var fileEntryMemoryStream = new MemoryStream())
            {
                StreamUtils.Copy(zipInputStream, fileEntryMemoryStream, buffer);
                fileEntries.Add(new FileEntry(zipEntry.Name.EnsureStartsWith('/'), fileEntryMemoryStream.ToArray(), zipEntry.IsDirectory));
            }

            zipEntry = zipInputStream.GetNextEntry();
        }

        return fileEntries;
    }
}