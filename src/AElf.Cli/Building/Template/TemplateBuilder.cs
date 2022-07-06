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
        var pipelines = new List<ITemplateBuildStep>
        {
            new TemplateBuildStepDownloadOrReadCachedTemplateFilesZip(),
            new TemplateBuildStepReadFileEntryList(),
            //new TemplateBuildStepRemoveProjectFromSolution(
            //    "CompanyName.ProjectName.Web.Tests"
            //),
            //new TemplateBuildStepRemoveProjectFromSolution(
            //    "CompanyName.ProjectName.Web"
            //),
            //new TemplateBuildStepChangeProjectName(TemplateBuildContext.OriginalFullName, _args.FullName),
            new TemplateBuildStepChangeSolutionName(
                TemplateBuildContext.OriginalCompanyName, 
                TemplateBuildContext.OriginalProjectName, 
                _args.CompanyName, 
                _args.ProjectName),
            new TemplateBuildStepCreateProjectResultZip(),
        };

        return pipelines;
    }
}