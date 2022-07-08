using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace CompanyName.ProjectName.Pages;

public class Index_Tests : ProjectNameWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
