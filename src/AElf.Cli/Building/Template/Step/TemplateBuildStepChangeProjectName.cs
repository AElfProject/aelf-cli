using System.Linq;
using AElf.Cli.Building.Utils;

namespace AElf.Cli.Building.Template.Step;

public class TemplateBuildStepChangeProjectName: ITemplateBuildStep
{
    private readonly string _oldProjectName;
    
    private readonly string _newProjectName;

    public TemplateBuildStepChangeProjectName(
        string oldProjectName,
        string newProjectName)
    {
        _oldProjectName = oldProjectName;
        _newProjectName = newProjectName;
    }
    
    public void Execute(TemplateBuildContext context)
    {
        RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldProjectName, _newProjectName);
    }

}