using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Services;

public interface IGeneratingService
{
    void Generate(string projectName, string path);
}

public class GeneratingService : IGeneratingService, ITransientDependency
{
    private const string TemplateFolderName = "template";
    private const string GeneratedFolderName = "generated";
    private const string ProjectPlaceholder = "HelloWorld";
    private const string ContractPlaceholder = "AElf.Contracts.HelloWorld";
    private const string ProtobufPlaceholder = "hello_world";

    private readonly List<string> _hiddenFiles = new() { "gitignore" };

    private readonly List<string> _replaceExtensions = new()
        { ".cs", ".proto", ".csproj", ".json", ".config", ".sln", ".cake", ".targets" };

    public GeneratingService()
    {
        Logger = NullLogger<GeneratingService>.Instance;
    }

    public ILogger<GeneratingService> Logger { get; set; }

    public void Generate(string projectName, string path)
    {
        var replacements = GetReplacements(projectName);
        path = GetGeneratedFolderPath(path);

        var generatedFiles = new Queue<string>();
        var originDir = new DirectoryInfo(GetTemplatePath());
        var destDir = CreateDir(path);

        var queue = new Queue<DirectoryInfo>();
        queue.Enqueue(originDir);
        do
        {
            var dir = queue.Dequeue();
            foreach (var directoryInfo in dir.GetDirectories())
            {
                queue.Enqueue(directoryInfo);

                var destSubDir = destDir.FullName + directoryInfo.FullName.Replace(originDir.FullName, "");
                CreateDir(ReplaceContent(destSubDir, replacements));
            }

            var files = dir.GetFiles();
            foreach (var originFile in files)
            {
                var destFileName = originFile.FullName.Replace(originDir.FullName, "");
                destFileName = ReplaceContent(destFileName, replacements);
                destFileName = ReplaceFileName(destFileName);

                destFileName = destDir.FullName + destFileName;
                originFile.CopyTo(destFileName, true);

                generatedFiles.Enqueue(destFileName);
            }
        } while (queue.Count > 0);

        while (generatedFiles.TryDequeue(out var file))
        {
            var extension = Path.GetExtension(file);
            if (extension != null && _replaceExtensions.Contains(extension))
            {
                var content = File.ReadAllText(file);
                content = ReplaceContent(content, replacements);
                File.WriteAllText(file, content);
            }
        }
    }

    private static DirectoryInfo CreateDir(string path)
    {
        return Directory.Exists(path) ? new DirectoryInfo(path) : Directory.CreateDirectory(path);
    }

    private string ReplaceContent(string input, List<Tuple<string, string>> replacements)
    {
        return replacements.Aggregate(input,
            (current, replacement) => current.Replace(replacement.Item1, replacement.Item2));
    }

    private string ReplaceFileName(string input)
    {
        return _hiddenFiles.Aggregate(input, (current, f) => current.Replace(f, "." + f));
    }

    private List<Tuple<string, string>> GetReplacements(string projectName)
    {
        var replacement = new List<Tuple<string, string>>
        {
            new(ContractPlaceholder, projectName)
        };

        var lastName = projectName.Split(".").Last();
        replacement.Add(new Tuple<string, string>(ProjectPlaceholder, lastName));
        replacement.Add(new Tuple<string, string>(ProtobufPlaceholder, GetProtoName(lastName)));

        return replacement;
    }

    private string GetProtoName(string name)
    {
        var sb = new StringBuilder(name.Length);
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (i != 0 && char.IsUpper(c)) sb.Append("_");

            sb.Append(char.ToLower(c));
        }

        return sb.ToString();
    }

    private string GetGeneratedFolderPath(string path)
    {
        if (path.IsNullOrWhiteSpace()) path = Directory.GetCurrentDirectory();

        return Path.Combine(path, GeneratedFolderName);
    }

    private string GetTemplatePath()
    {
        var type = typeof(GeneratingService);
        var currentDirectory = Path.GetDirectoryName(type.Assembly.Location);
        return Path.Combine(currentDirectory, TemplateFolderName);
    }
}