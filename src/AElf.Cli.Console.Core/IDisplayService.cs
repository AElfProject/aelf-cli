namespace AElf.Cli.Console.Core;

public interface IDisplayService
{
    /// <summary>
    /// Display an option list and return the selected option.
    /// </summary>
    /// <param name="optionList"></param>
    /// <returns></returns>
    string Select(IEnumerable<string> optionList);

    T Ask<T>(string question);

    bool Confirm(string question);

    T Prompt<T>(string title, IEnumerable<T> optionList) where T : notnull;

    void WriteLine();

    void MarkupLine(string value);
}