using System.Collections.Generic;
using AElf.Cli.Building.Template.Step;
using AElf.Cli.Commands;

namespace AElf.Cli.Building.Template;

public class TemplateBuilder
{
    private readonly TemplateBuildContext _context;
    private readonly NewCommand.NewCommandArgs _args;

    public TemplateBuilder(NewCommand.NewCommandArgs args)
    {
        _context = new TemplateBuildContext(args);
        _args = args;
    }
    
    public TemplateBuildResult Build()
    {
        foreach (var step in BuildPipeline())
        {
            step.Execute(_context);
        }
        return new TemplateBuildResult(_context);
    }

    private IEnumerable<ITemplateBuildStep> BuildPipeline()
    {
        //根据不同的template name和参数构建不同的pipeline
        var pipelines = new List<ITemplateBuildStep>
        {
            new TemplateBuildStepDownloadOrReadCachedTemplateFilesZip(),
            new TemplateBuildStepReadFileEntryList(),
            new TemplateBuildStepChangeProjectName(TemplateBuildContext.OriginalProjectName, _args.ProjectName),
            new TemplateBuildStepCreateProjectResultZip(),
        };

        return pipelines;
    }
}