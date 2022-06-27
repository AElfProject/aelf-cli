using AElf.Cli.Building.File;

namespace AElf.Cli.Building.Project.Step;

public class ProjectBuildStepExtractProjectZip:IProjectBuildStep
{
    private readonly string _outputFolder;
    
    public ProjectBuildStepExtractProjectZip(string outputFolder)
    {
        _outputFolder = outputFolder;
    }

    public void Execute(ProjectBuildContext context)
    {
        FileHelper.ExtractProjectZip(context.ZipContent, _outputFolder);
    }
}