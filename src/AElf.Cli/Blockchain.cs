using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AElf.Cli
{
    public class Blockchain
    {
        private static void RegisterAssemblyResolveEvent()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            if (!File.Exists(assemblyPath)) return null;
            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        public static void Start()
        {
            RegisterAssemblyResolveEvent();
            ILogger<Blockchain> logger = NullLogger<Blockchain>.Instance;
            try
            {
                CreateHostBuilder().Build().Run();
            }
            catch (Exception e)
            {
                if (logger == NullLogger<Blockchain>.Instance)
                    Console.WriteLine(e);
                logger.LogCritical(e, "program crashed");
            }
        }

        // create default https://github.com/aspnet/MetaPackages/blob/master/src/Microsoft.AspNetCore/WebHost.cs
        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureLogging(builder => { builder.ClearProviders(); })
                .ConfigureAppConfiguration(build =>
                {
                    var type = typeof(Blockchain);
                    var currentDirectory = Path.GetDirectoryName(type.Assembly.Location);
                    build.SetBasePath(currentDirectory);
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseAutofac();
    }
}