namespace AElf.Cli.Building.Project.Step;

public interface IProjectBuildStep
{ 
    void Execute(ProjectBuildContext context);
}