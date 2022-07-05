using System.Xml;
using AElf.Cli.Building.Utils;

namespace AElf.Cli.Building.Template.Step;

public class TemplateBuildStepRemoveProjectReference: ITemplateBuildStep
{
    private readonly string _projectName;
    private string _sourceProject;

    public TemplateBuildStepRemoveProjectReference(
        string projectName,
        string sourceProject = null)
    {
        _projectName = projectName;
        _sourceProject = sourceProject;
    }
    public void Execute(TemplateBuildContext context)
    {
        foreach (var fileEntry in context.FileEntryList)
        {
            if (fileEntry.Name.EndsWith(".csproj") && (string.IsNullOrWhiteSpace(_sourceProject) || fileEntry.Name.Contains(_sourceProject)))
            {
                fileEntry.SetContent(ProcessFileContent(fileEntry.Content));
            }
        }
    }
    
    private string ProcessFileContent(string content)
    {
        using (var stream = StreamHelper.GenerateStreamFromString(content))
        {
            var doc = new XmlDocument() { PreserveWhitespace = true };
            doc.Load(stream);
            return ProcessReferenceNodes(doc);
        }
    }
    
    private string ProcessReferenceNodes(XmlDocument doc)
    {
        var nodes = doc.SelectNodes("/Project/ItemGroup/ProjectReference[@Include]");

        foreach (XmlNode node in nodes)
        {
            var nodeIncludeValue = node.Attributes["Include"].Value;

            if (nodeIncludeValue.Contains(_projectName))
            {
                var parentNode = node.ParentNode;
                parentNode.RemoveChild(node);
            }
        }

        return doc.OuterXml;
    }
    
}