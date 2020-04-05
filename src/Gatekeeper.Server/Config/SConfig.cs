#region

using System;
using System.Collections.Generic;
using System.Linq;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Remote;
using Hexagon.Config;
using Hexagon.Logging;
using Hexagon.Utilities;
using Tomlyn.Model;

#endregion

namespace Gatekeeper.Server.Config {
    public class SConfig : ServerConfiguration {
        public const string SERVER_NAME = "ALTiCU Gatekeeper.Server";
        public const string VERSION = "0.1.0-dev";

        public class Server {
            public const string DEFAULT_DATABASE = "Data Source=database.db";

            /// <summary>
            /// Database connection path.
            /// Specify a file using "Data Source=database.db"
            /// Specify in-memory transient using null
            /// </summary>
            public string database = DEFAULT_DATABASE;

#if DEBUG
            /// <summary>
            /// Enables development mode (sets up hardcoded/test values and "fake" services)
            /// </summary>
            public bool development = true;
#endif
        }

        public Server server = new Server();

        public List<RemoteApp> apps = new List<RemoteApp> { };

        public List<Group> groups = new List<Group>();

        public class Users {
            public List<string> defaultGroups = new List<string>();
        }

        public Users users = new Users();

        public class Logging {
            /// <summary>
            /// The verbosity of the application logger
            /// </summary>
            public SLogger.LogLevel logLevel = SLogger.LogLevel.Information;

            /// <summary>
            /// Whether to enable ASP.NET Core verbose logging
            /// </summary>
            public bool aspnetVerboseLogging = true;

            /// <summary>
            /// Whether to enable detailed/sensitive database logging
            /// </summary>
            public bool databaseLogging = true;
        }

        public Logging logging = new Logging();

        #region Configuration Loading

        public override void load(TomlTable tb) {
            var serverTable = tb.getField<TomlTable>(nameof(server));
#if DEBUG
            serverTable.bindField(ref server.development, nameof(Server.development));
#endif

            var appsTables = tb.getField<TomlTableArray>(nameof(apps));
            foreach (var appTable in appsTables) {
                var app = new RemoteApp {
                    name = appTable.getField<string>(nameof(RemoteApp.name)),
                    secret = appTable.getField<string>(nameof(RemoteApp.secret)),
                };
                app.layers = appTable.getField<TomlArray>(nameof(RemoteApp.layers))
                    .Select(x => x.ToString())
                    .ToList();
                apps.Add(app);
            }

            var groupsTables = tb.getField<TomlTableArray>(nameof(groups));
            foreach (var groupTable in groupsTables) {
                var group = new Group {
                    name = groupTable.getField<string>(nameof(Group.name)),
                    priority = groupTable.getField<long>(nameof(Group.priority))
                };
                // validate group name
                if (!StringValidator.isIdentifier(group.name)) {
                    throw new FormatException($"group name {group.name} is not a valid identifier.");
                }

                // load permissions and rules
                group.permissions = groupTable.getField<TomlArray>(nameof(Group.permissions))
                    .Select(x => new Permission(x.ToString()))
                    .ToList();
                var groupRules = groupTable.getField<TomlTable>(nameof(Group.rules));
                foreach (var appRulesTablePair in groupRules) {
                    var ruleApp = appRulesTablePair.Key;
                    var rulesTable = (TomlTable) appRulesTablePair.Value;
                    foreach (var appRule in rulesTable) {
                        group.rules.Add(new AccessRule(ruleApp, appRule.Key, appRule.Value.ToString()));
                    }
                }

                groups.Add(group);
            }

            var usersTable = tb.getField<TomlTable>(nameof(users));
            usersTable.bindField(ref users.defaultGroups, nameof(users.defaultGroups));

            var loggingTable = tb.getField<TomlTable>(nameof(logging));
            loggingTable.bindField(ref logging.logLevel, nameof(logging.logLevel));
            loggingTable.bindField(ref logging.aspnetVerboseLogging, nameof(logging.aspnetVerboseLogging));
            loggingTable.bindField(ref logging.databaseLogging, nameof(logging.databaseLogging));
        }

        #endregion
    }
}