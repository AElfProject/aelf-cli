namespace AElf.Cli.Building.Template.Step;

public interface ITemplateBuildStep
{ 
    void Execute(TemplateBuildContext context);
}