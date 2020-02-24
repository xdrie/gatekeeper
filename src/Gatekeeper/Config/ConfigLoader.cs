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

        public static void bindField<T>(this TomlTable table, ref T target, string fieldName) {
            if (table.ContainsKey(fieldName)) {
                target = table.getField<T>(fieldName);
            }
        }

        public static SConfig readDocument(TomlTable tb) {
            var cfg = new SConfig();

            var server = tb.getField<TomlTable>(nameof(cfg.server));
#if DEBUG
            server.bindField(ref cfg.server.development, nameof(cfg.server.development));
#endif

            var apps = tb.getField<TomlTableArray>(nameof(cfg.apps));
            foreach (var app in apps) {
                cfg.apps.Add(new SConfig.RemoteApp {
                    name = app.getField<string>(nameof(SConfig.RemoteApp.name))
                });
            }

            var logging = tb.getField<TomlTable>(nameof(cfg.logging));
            logging.bindField(ref cfg.logging.logLevel, nameof(cfg.logging.logLevel));
            logging.bindField(ref cfg.logging.aspnetVerboseLogging, nameof(cfg.logging.aspnetVerboseLogging));
            logging.bindField(ref cfg.logging.databaseLogging, nameof(cfg.logging.databaseLogging));

            return cfg;
        }
    }
}