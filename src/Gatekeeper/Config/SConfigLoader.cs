#region

using System;
using System.Linq;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Remote;
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
                var appCfg = new RemoteApp {
                    name = app.getField<string>(nameof(RemoteApp.name)),
                    secret = app.getField<string>(nameof(RemoteApp.secret)),
                };
                appCfg.layers = app.getField<TomlArray>(nameof(RemoteApp.layers))
                    .Select(x => x.ToString())
                    .ToList();
                cfg.apps.Add(appCfg);
            }
            
            var groups = tb.getField<TomlTableArray>(nameof(cfg.groups));
            foreach (var group in groups) {
                var groupCfg = new Group {
                    name = group.getField<string>(nameof(Group.name))
                };
                // validate group name
                if (!StringValidator.isIdentifier(groupCfg.name)) {
                    throw new FormatException($"group name {groupCfg.name} is not a valid identifier.");
                }
                cfg.groups.Add(groupCfg);
            }

            var users = tb.getField<TomlTable>(nameof(cfg.users));
            users.bindField(ref cfg.users.defaultGroups, nameof(cfg.users.defaultGroups));

            var logging = tb.getField<TomlTable>(nameof(cfg.logging));
            logging.bindField(ref cfg.logging.logLevel, nameof(cfg.logging.logLevel));
            logging.bindField(ref cfg.logging.aspnetVerboseLogging, nameof(cfg.logging.aspnetVerboseLogging));
            logging.bindField(ref cfg.logging.databaseLogging, nameof(cfg.logging.databaseLogging));

            return cfg;
        }
    }
}