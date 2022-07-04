using System;
using System.Collections.Generic;
using System.Linq;

namespace AElf.Cli.Building.Template.Step;

public class TemplateBuildStepRemoveFolder:ITemplateBuildStep
{
    private readonly string _folderPath;

    public TemplateBuildStepRemoveFolder(string folderPath)
    {
        _folderPath = folderPath;
    }

    public void Execute(TemplateBuildContext context)
    {
        //Remove the folder content
        var folderPathWithSlash = _folderPath.EnsureEndsWith('/');
        context.FileEntryList.RemoveAll(file => file.Name.StartsWith(folderPathWithSlash));

        //Remove the folder
        var folder = context.FileEntryList.FirstOrDefault(file => file.Name == _folderPath);
        if (folder != null)
        {
            context.FileEntryList.Remove(folder);
        }
    }
}