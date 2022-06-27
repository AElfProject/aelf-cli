namespace AElf.Cli.Building.Template;

public class TemplateBuildResult
{
    public byte[] ZipContent { get; }

    public TemplateBuildResult(TemplateBuildContext context)
    {
        ZipContent = context.ProjectZipContent;
    }
}