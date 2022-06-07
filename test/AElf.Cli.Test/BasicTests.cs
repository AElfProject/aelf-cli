using System.Threading.Tasks;
using Xunit;

namespace AElf.Cli.Test;

public class BasicTests : AElfCliTestBase
{
    [Fact]
    public async Task Test1()
    {
        await CliService.RunAsync(new[] { "create" });
    }
}