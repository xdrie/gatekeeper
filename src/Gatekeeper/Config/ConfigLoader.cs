#region

using System.Text;
using Hexagon.Services.Application;
using Tomlyn.Model;

#endregion

namespace Gatekeeper.Config {
    public static class ConfigLoader {
        private static string rename(string name) {
            var sb = new StringBuilder();
            foreach (var ch in name) {
                if (char.IsUpper(ch))
                    sb.Append("_");

                sb.Append(ch.ToString().ToLower());
            }

            return sb.ToString();
        }

        public static T getField<T>(this TomlTable table, string field) {
            return (T) table[rename(field)];
        }

        public static SConfig readDocument(TomlTable tb) {
            var cfg = new SConfig();

            var server = tb.getField<TomlTable>(nameof(cfg.server));
#if DEBUG
            cfg.server.development = server.getField<bool>(nameof(cfg.server.development));
#endif

            var apps = (TomlTableArray) tb[rename(nameof(cfg.apps))];
            foreach (var app in apps) {
                cfg.apps.Add(new SConfig.RemoteApp {
                    name = (string) app[rename(nameof(SConfig.RemoteApp.name))]
                });
            }

            var logging = (TomlTable) tb[rename(nameof(cfg.logging))];
            cfg.logging.logLevel = (SLogger.LogLevel) logging[rename(nameof(cfg.logging.logLevel))];
            cfg.logging.aspnetVerboseLogging = (bool) logging[rename(nameof(cfg.logging.aspnetVerboseLogging))];
            cfg.logging.databaseLogging = (bool) logging[rename(nameof(cfg.logging.databaseLogging))];

            return cfg;
        }
    }
}