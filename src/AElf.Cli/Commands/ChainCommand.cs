using System;
using System.Threading.Tasks;
using AElf.Cli.Args;

namespace AElf.Cli.Commands;

public class ChainCommand : IAElfCommand
{
    public Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        return Task.CompletedTask;
    }

    public string GetUsageInfo()
    {
        return "Not implemented yet.";
    }

    public string GetShortDescription()
    {
        return "Not implemented yet.";
    }
}