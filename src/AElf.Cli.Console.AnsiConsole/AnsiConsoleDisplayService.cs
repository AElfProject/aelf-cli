using AElf.Cli.Console.Core;
using Spectre.Console;
using Volo.Abp.DependencyInjection;

namespace AElf.Cli.Console.AnsiConsole;

public class DisplayService : IDisplayService, ITransientDependency
{
    public string Select(IEnumerable<string> optionList)
    {
        return Spectre.Console.AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(AElfCliConsoleConstants.ChooseAMethod)
                .PageSize(AElfCliConsoleConstants.DefaultPageSize)
                .MoreChoicesText(AElfCliConsoleConstants.MoreMethods)
                .AddChoices(optionList));
    }

    public T Ask<T>(string question)
    {
        return Spectre.Console.AnsiConsole.Ask<T>(question);
    }

    public bool Confirm(string question)
    {
        return Spectre.Console.AnsiConsole.Confirm(question);
    }

    public T Prompt<T>(string title, IEnumerable<T> optionList) where T : notnull
    {
        return Spectre.Console.AnsiConsole.Prompt(
            new SelectionPrompt<T>()
                .Title(title)
                .PageSize(AElfCliConsoleConstants.DefaultPageSize)
                .AddChoices(optionList)
        );
    }

    public void WriteLine()
    {
        Spectre.Console.AnsiConsole.WriteLine();
    }

    public void MarkupLine(string value)
    {
        Spectre.Console.AnsiConsole.MarkupLine(value);
    }
}