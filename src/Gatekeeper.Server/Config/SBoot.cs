using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Iri.Glass.Logging;
using Microsoft.AspNetCore.Hosting;

#if !DEBUG
using Microsoft.Extensions.Hosting;
#endif

namespace Gatekeeper.Server.Config {
    public class SBoot {
        public const string BOOT_BANNER = "Gatekeeper.Server.Data.boot.txt";

        public static void display(SContext context, IWebHostEnvironment env) {
            var bootBannerRes = Assembly.GetExecutingAssembly().GetManifestResourceStream(BOOT_BANNER);
            using (var sr = new StreamReader(bootBannerRes)) {
                context.log.writeLine($"\n{sr.ReadToEnd()}\nv{SConfig.VERSION} instance '{SConfig.SERVER_NAME}'",
                    Logger.Verbosity.Information);
            }

#if DEBUG
            // print debug banner (always)
            context.log.writeLine(
                $"this is a DEBUG build of {nameof(Server)}. this build should NEVER be used in production.",
                Logger.Verbosity.Error);
            if (context.config.server.development) {
                context.log.writeLine(
                    $"development/test mode is enabled. default values and fake external services will be used.",
                    Logger.Verbosity.Error);
            }
#else
                if (!env.IsProduction()) {
                    context.log.writeLine(
                        $"this is a release build of {nameof(Gatekeeper.Server)}, but is not being run in Production (it is being run in '{env.EnvironmentName}')",
                        Logger.Verbosity.Error);
                }
#endif

            var configuredAppNames = string.Join(", ", context.config.apps.Select(x => x.name));
            context.log.writeLine(
                $"available remote application configurations[{context.config.apps.Count}]: remote applications are configured: [{configuredAppNames}]",
                Logger.Verbosity.Information);
        }
    }
}