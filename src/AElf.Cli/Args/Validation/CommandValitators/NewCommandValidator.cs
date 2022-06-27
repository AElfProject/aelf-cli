using System;
using System.Text;
using AElf.Cli.Commands;

namespace AElf.Cli.Args.Validation.CommandValitators;

public class NewCommandValidator
{
    public static bool Validate(CommandLineArgs args, string usageInfo)
    {
        var commandErrorString = ValidateCommand(args.Command, args.Target);
        var optionsErrorString = ValidateOptions(args.Options);

        if (!string.IsNullOrWhiteSpace(commandErrorString)
            || !string.IsNullOrWhiteSpace(optionsErrorString))
        {
            throw new AElfCliUsageException(
                commandErrorString + optionsErrorString +
                Environment.NewLine + Environment.NewLine +
                usageInfo
            );
        }
        return true;
    }

    private static string ValidateCommand(string command, string target)
    {
        return target.IsNullOrWhiteSpace() ? "Project name is missing!" : string.Empty;
    }

    private static string ValidateOptions(CommandLineOptions options)
    {
        var error = new StringBuilder();
        foreach (var option in options)
        {
            if (NewCommand.NewCommandOptions.OutputFolder.Short.Equals(option.Key, StringComparison.CurrentCultureIgnoreCase)
                || NewCommand.NewCommandOptions.OutputFolder.Long.Equals(option.Key, StringComparison.CurrentCultureIgnoreCase))
            {
                //检查option value的合法性
                if (string.IsNullOrWhiteSpace(option.Value))
                {
                    error.AppendLine($"\tplease give a value for option: -${option.Key}");
                }
            }
            else
            {
                error.AppendLine($"\toption -${option.Key} is not valid for New Command!");
            }
        }

        var errorString = error.ToString();
        return errorString.Length >0 ? errorString : string.Empty;
    }
}