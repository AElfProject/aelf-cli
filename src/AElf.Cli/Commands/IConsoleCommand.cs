using System.Threading.Tasks;
using AElf.Cli.Args;

namespace AElf.Cli.Commands
{
    public interface IConsoleCommand
    {
        Task ExecuteAsync(CommandLineArgs commandLineArgs);

        string GetUsageInfo();

        string GetShortDescription();
    }
}