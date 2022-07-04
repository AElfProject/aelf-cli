using System;
using System.Linq;
using AElf.Cli.Building.File;

namespace AElf.Cli.Building.Template;

public static class TemplateBuildContextExtensions
{
    public static FileEntry GetFile(this TemplateBuildContext context, string filePath)
    {
        var file = context.FileEntryList.FirstOrDefault(f => f.Name == filePath);
        if (file == null)
        {
            throw new ApplicationException("Could not find file: " + filePath);
        }

        return file;
    }

    public static FileEntry FindFile(this TemplateBuildContext context, string filePath)
    {
        return context.FileEntryList.FirstOrDefault(f => f.Name == filePath);
    }
}