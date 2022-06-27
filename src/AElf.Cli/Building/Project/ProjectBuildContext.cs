namespace AElf.Cli.Building.Project;

public class ProjectBuildContext
{
    public byte[] ZipContent { get; }

    public string OutputFolder { get; }
    
    public ProjectBuildContext(
        byte[] zipContent,
        string outputFolder)
    {
        ZipContent = zipContent;
        OutputFolder = outputFolder;
    }

}