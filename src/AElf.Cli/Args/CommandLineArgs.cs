using System;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace AElf.Cli.Args;

public class CommandLineArgs
{
    public CommandLineArgs([CanBeNull] string command = null, [CanBeNull] string target = null)
    {
        Command = command;
        Target = target;
        Options = new CommandLineOptions();
    }

    [CanBeNull] public string Command { get; }

    [CanBeNull] public string Target { get; }

    [NotNull] public CommandLineOptions Options { get; }

    public static CommandLineArgs Empty()
    {
        return new CommandLineArgs();
    }

    public bool IsCommand(string command)
    {
        return string.Equals(Command, command, StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (Command != null) sb.AppendLine($"Command: {Command}");

        if (Target != null) sb.AppendLine($"Target: {Target}");

        if (Options.Any())
        {
            sb.AppendLine("Options:");
            foreach (var (key, value) in Options) sb.AppendLine($" - {key} = {value}");
        }

        if (sb.Length <= 0) sb.Append("<EMPTY>");

        return sb.ToString();
    }
}