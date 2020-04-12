using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Serilog;

#if !DEBUG
using Microsoft.Extensions.Hosting;
#endif

namespace Gatekeeper.Server.Config {
    public class SBoot {
        public const string BOOT_BANNER = "Gatekeeper.Server.Data.boot.txt";

        public static void display(SContext context, IWebHostEnvironment env) {
            var bootBannerRes = Assembly.GetExecutingAssembly().GetManifestResourceStream(BOOT_BANNER);
            using (var sr = new StreamReader(bootBannerRes)) {
                Console.WriteLine(sr.ReadToEnd());
                Log.Information("v{Version} instance '{ServerName}'", SConfig.VERSION, SConfig.SERVER_NAME);
            }

#if DEBUG
            // print debug banner (always)
            Log.Warning("THIS IS A DEBUG BUILD OF {ServerName}. DO NOT USE IN PROD.", nameof(Server));
            if (context.config.server.development) {
                Log.Warning("Dev/Test mode is enabled. Default values and fake external services will be used.");
            }
#else
                if (!env.IsProduction()) {
                    Log.Error("This is a release build of {ServerName}, but is not being run in Prod (being run in '{EnvName}')",
                        nameof(GateKeeper.server), env.EnvironmentName);
                }
#endif

            var configuredAppNames = string.Join(", ", context.config.apps.Select(x => x.name));
            Log.Information(
                "Availible remote application configurations[{configCount}]: remote apps are configured: [{configNames}]",
                context.config.apps.Count, configuredAppNames);
        }
    }
}