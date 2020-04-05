#region

using System.Linq;
using Hexagon.Utilities;
using Tomlyn.Model;

#endregion

namespace Gatekeeper.Config {
    public class ConfigLoader {
        public static SConfig readDocument(TomlTable tb) {
            var cfg = new SConfig();

            var server = tb.getField<TomlTable>(nameof(cfg.server));
#if DEBUG
            server.bindField(ref cfg.server.development, nameof(cfg.server.development));
#endif

            var apps = tb.getField<TomlTableArray>(nameof(cfg.apps));
            foreach (var app in apps) {
                var appCfg = new SConfig.RemoteApp {
                    name = app.getField<string>(nameof(SConfig.RemoteApp.name)),
                    secret = app.getField<string>(nameof(SConfig.RemoteApp.secret)),
                };
                appCfg.layers = app.getField<TomlArray>(nameof(SConfig.RemoteApp.layers)).Select(x => x.ToString())
                    .ToList();
                cfg.apps.Add(appCfg);
            }

            var users = tb.getField<TomlTable>(nameof(cfg.users));
            users.bindField(ref cfg.users.defaultLayers, nameof(cfg.users.defaultLayers));

            var logging = tb.getField<TomlTable>(nameof(cfg.logging));
            logging.bindField(ref cfg.logging.logLevel, nameof(cfg.logging.logLevel));
            logging.bindField(ref cfg.logging.aspnetVerboseLogging, nameof(cfg.logging.aspnetVerboseLogging));
            logging.bindField(ref cfg.logging.databaseLogging, nameof(cfg.logging.databaseLogging));

            return cfg;
        }
    }
}