using System;
using System.Linq;
using AElf.Cli.Building.Utils;

namespace AElf.Cli.Building.Template.Step;

public class TemplateBuildStepChangeSolutionName: ITemplateBuildStep
{
    private readonly string _oldCompanyName;
    private readonly string _oldProjectName;
    private readonly string _newCompanyName;
    private readonly string _newProjectName;

    public TemplateBuildStepChangeSolutionName(
        string oldCompanyName,
        string oldProjectName,
        string newCompanyName,
        string newProjectName)
    {
        _oldCompanyName = oldCompanyName;
        _oldProjectName = oldProjectName;
        _newCompanyName = newCompanyName;
        _newProjectName = newProjectName;
    }
    
    public void Execute(TemplateBuildContext context)
    {
        if (_newCompanyName != null)
        {
            RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldCompanyName, _newCompanyName);
            RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldCompanyName.ToCamelCase(), _newCompanyName.ToCamelCase());
            RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldCompanyName.ToKebabCase(), _newCompanyName.ToKebabCase());
            RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldCompanyName.ToLowerInvariant(), _newCompanyName.ToLowerInvariant());
        }
        else
        {
            RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldCompanyName + "." + _oldProjectName, _oldProjectName);
            RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldCompanyName.ToCamelCase() + "." + _oldProjectName.ToCamelCase(), _oldProjectName.ToCamelCase());
            RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldCompanyName.ToLowerInvariant() + "." + _oldProjectName.ToLowerInvariant(), _oldProjectName.ToLowerInvariant());
            RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldCompanyName.ToKebabCase() + "/" + _oldProjectName.ToKebabCase(), _oldProjectName.ToKebabCase());
        }

        RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldProjectName, _newProjectName);
        RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldProjectName.ToCamelCase(), _newProjectName.ToCamelCase());
        RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldProjectName.ToKebabCase(), _newProjectName.ToKebabCase());
        RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldProjectName.ToLowerInvariant(), _newProjectName.ToLowerInvariant());
        RenameHelper.RenameAll(context.FileEntryList.ToList(), _oldProjectName.ToSnakeCase().ToUpper(), _newProjectName.ToSnakeCase().ToUpper());
    }

}