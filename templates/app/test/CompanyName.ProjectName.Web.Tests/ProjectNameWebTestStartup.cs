using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace CompanyName.ProjectName;

public class ProjectNameWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<ProjectNameWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
