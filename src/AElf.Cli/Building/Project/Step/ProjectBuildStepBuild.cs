using AElf.Cli.Building.Utils;

namespace AElf.Cli.Building.Project.Step;

public class ProjectBuildStepBuild: IProjectBuildStep
{
    public void Execute(ProjectBuildContext context)
    {
        CmdHelper.RunCmd("dotnet build /graphbuild", context.OutputFolder);
    }
}