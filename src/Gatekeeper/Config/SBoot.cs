using System.IO;
using System.Reflection;
using Hexagon.Services.Application;

namespace Gatekeeper.Config {
    public class SBoot {
        public const string BOOT_BANNER = "Gatekeeper.Data.boot.txt";

        public static void display(SContext context) {
            var bootBannerRes = Assembly.GetExecutingAssembly().GetManifestResourceStream(BOOT_BANNER);
            using (var sr = new StreamReader(bootBannerRes)) {
                context.log.writeLine($"\n{sr.ReadToEnd()}\nv{SConfig.VERSION}", SLogger.LogLevel.Information);
            }

            context.log.writeLine($"running {nameof(Gatekeeper)} {SConfig.VERSION} instance '{SConfig.SERVER_NAME}'",
                SLogger.LogLevel.Information);
        }
    }
}