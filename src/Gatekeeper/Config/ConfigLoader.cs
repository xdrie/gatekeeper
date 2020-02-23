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

        public static SConfig readDocument(TomlTable tb) {
            var cfg = new SConfig();

            var server = (TomlTable) tb[rename(nameof(cfg.server))];
            cfg.server.production = (bool) server[rename(nameof(cfg.server.production))];

            var logging = (TomlTable) tb[rename(nameof(cfg.logging))];
            cfg.logging.logLevel = (SLogger.LogLevel) logging[rename(nameof(cfg.logging.logLevel))];
            cfg.logging.aspnetVerboseLogging = (bool) logging[rename(nameof(cfg.logging.aspnetVerboseLogging))];
            cfg.logging.databaseLogging = (bool) logging[rename(nameof(cfg.logging.databaseLogging))];

            return cfg;
        }
    }
}