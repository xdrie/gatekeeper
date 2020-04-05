using System.IO;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gatekeeper.Server {
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class Program {
        public static void Main(string[] args) {
            var host = CreateWebHostBuilder(args).Build();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            // boot ASP.NET Core app
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "hosting.json"), true)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();

            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(logging => { // add aspnet logger
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .UseStartup<Startup>();
        }
    }
}