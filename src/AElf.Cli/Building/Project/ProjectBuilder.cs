using System.Collections.Generic;
using AElf.Cli.Building.Project.Step;
using AElf.Cli.Building.Template;
using AElf.Cli.Commands;

namespace AElf.Cli.Building.Project;

public class ProjectBuilder
{
    private readonly ProjectBuildContext _context;
    
    public ProjectBuilder(TemplateBuildResult templateBuildResult, NewCommand.NewCommandArgs args)
    {
        _context = new ProjectBuildContext(templateBuildResult.ZipContent, args.OutputFolder);
    }

    public ProjectBuildResult Build()
    {
        foreach (var step in BuildPipeline())
        {
            step.Execute(_context);
        }
        return new ProjectBuildResult(_context);
    }

    private IEnumerable<IProjectBuildStep> BuildPipeline()
    {
        //根据不同的template name和参数信息构建不同的pipeline
        var pipelines = new List<IProjectBuildStep>
        {
            new ProjectBuildStepExtractProjectZip(_context.OutputFolder),
            new ProjectBuildStepBuild(),
        };

        return pipelines;
    }
    
}