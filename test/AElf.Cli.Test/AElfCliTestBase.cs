using AElf.Cli.Args;
using AElf.Cli.Commands;
using AElf.Cli.TestBase;
using Microsoft.Extensions.DependencyInjection;

namespace AElf.Cli.Test;

public class AElfCliTestBase : AElfCliIntegratedTest<AElfTestModule>
{
    internal readonly AElfCliService CliService;

    public AElfCliTestBase()
    {
        CliService = new AElfCliService(GetRequiredService<ICommandLineArgumentParser>(),
            GetRequiredService<ICommandSelector>(), GetRequiredService<IServiceScopeFactory>());
    }
}