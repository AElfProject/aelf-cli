namespace AElf.Cli.Building.Project;

public class ProjectBuildResult
{
    public ProjectBuildContext Context { get; }

    public ProjectBuildResult(ProjectBuildContext context)
    {
        Context = context;
    }
}