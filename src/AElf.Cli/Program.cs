using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Volo.Abp;

namespace AElf.Cli;

public static class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
            .MinimumLevel.Override("AElf.Cli", LogEventLevel.Debug)
#else
            .MinimumLevel.Override("AElf.Cli", LogEventLevel.Information)
#endif
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File($"Logs/aelf-cli-{DateTime.UtcNow:yyyy-MM-dd}.logs"))
            .WriteTo.Console()
            .CreateLogger();

        using var application = AbpApplicationFactory.Create<AElfCliModule>(
            options =>
            {
                options.UseAutofac();
                options.Services.AddLogging(c => c.AddSerilog());
            });
        await application.InitializeAsync();

        await application.ServiceProvider
            .GetRequiredService<AElfCliService>()
            .RunAsync(args);

        await application.ShutdownAsync();
        Log.CloseAndFlush();
    }
}