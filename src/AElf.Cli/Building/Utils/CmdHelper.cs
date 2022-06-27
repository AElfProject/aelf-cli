using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AElf.Cli.Building.Utils;

public static class CmdHelper
{
    private const int SuccessfulExitCode = 0;

    public static void Run(string file, string arguments)
    {
        var procStartInfo = new ProcessStartInfo(file, arguments);
        Process.Start(procStartInfo)?.WaitForExit();
    }

    public static void RunCmd(string command, string workingDirectory = null)
    {
        RunCmd(command, out _, workingDirectory);
    }

    private static void RunCmd(string command, out int exitCode, string workingDirectory = null)
    {
        var procStartInfo = new ProcessStartInfo(
            GetFileName(),
            GetArguments(command)
        );

        if (!string.IsNullOrEmpty(workingDirectory))
        {
            procStartInfo.WorkingDirectory = workingDirectory;
        }

        using var process = Process.Start(procStartInfo);
        process?.WaitForExit();

        exitCode = process?.ExitCode ?? SuccessfulExitCode;
    }

    private static string GetArguments(string command, int? delaySeconds = null)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return delaySeconds == null ? "-c \"" + command + "\"" : "-c \"" + $"sleep {delaySeconds}s > /dev/null && " + command + "\"";
        }

        //Windows default.
        return delaySeconds == null ? "/C \"" + command + "\"" : "/C \"" + $"timeout /nobreak /t {delaySeconds} >null && " + command + "\"";
    }

    private static string GetFileName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            //Windows
            return "cmd.exe";
        }

        //Linux or OSX
        if (System.IO.File.Exists("/bin/bash"))
        {
            return "/bin/bash";
        }

        if (System.IO.File.Exists("/bin/sh"))
        {
            return "/bin/sh"; //some Linux distributions like Alpine doesn't have bash
        }

        throw new AElfCliUsageException($"Cannot determine shell command for this OS! " +
                               $"Running on OS: {System.Runtime.InteropServices.RuntimeInformation.OSDescription} | " +
                               $"OS Architecture: {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture} | " +
                               $"Framework: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription} | " +
                               $"Process Architecture{System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture}");
    }
}
