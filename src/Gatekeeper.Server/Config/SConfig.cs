#region

using System;
using System.Collections.Generic;
using System.Linq;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Remote;
using Iri.Glass.Config;
using Iri.Glass.Logging;
using Iri.Glass.Utilities;
using Tomlyn.Model;

#endregion

namespace Gatekeeper.Server.Config {
    public class SConfig : TomlConfig {
        public const string BRAND = "GaTE";
        public const string SERVER_NAME = "ALTiCU Gatekeeper.Server";
        public const string VERSION = "0.4.4b";

        public class Server {
            public const string DEFAULT_DATABASE = "Data Source=database.db"; // Default Sqlite database

            /// <summary>
            /// Database connection path.
            /// </summary>
            public string databaseConnection = DEFAULT_DATABASE;

            public DatabaseBackend databaseBackend = DatabaseBackend.Sqlite;

            public enum DatabaseBackend {
                InMemory,
                Sqlite,
                Postgres
            }

#if DEBUG
            /// <summary>
            /// Enables development mode (sets up hardcoded/test values and "fake" services)
            /// </summary>
            public bool development = true;
#endif

            public List<string> cors = new List<string>();
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
            public Logger.Verbosity Verbosity = Logger.Verbosity.Information;

            /// <summary>
            /// Whether to enable ASP.NET Core verbose logging
            /// </summary>
            public bool aspnetVerboseLogging = true;

            /// <summary>
            /// Whether to enable detailed/sensitive database logging
            /// </summary>
            public bool databaseLogging = false;
        }

        public Logging logging = new Logging();

        #region Configuration Loading

        protected override void load(TomlTable tb) {
            var serverTable = tb.getTable(nameof(server));
            serverTable.autoBind(server);

            var appsTables = tb.getTableArray(nameof(apps));
            foreach (var appTable in appsTables) {
                var cfgApp = new RemoteApp();
                appTable.autoBind(cfgApp);
                // validate app
                if (!StringValidator.isIdentifier(cfgApp.name)) {
                    throw new ConfigurationException(nameof(cfgApp.name), $"app name '{cfgApp.name}' is not valid");
                }

                if (string.IsNullOrEmpty(cfgApp.secret)) {
                    throw new ConfigurationException(nameof(cfgApp.secret), "app secret cannot be empty");
                }

                apps.Add(cfgApp);
            }

            var groupsTables = tb.getTableArray(nameof(groups));
            foreach (var groupTable in groupsTables) {
                var cfgGroup = new Group {
                    name = groupTable.getField<string>(nameof(Group.name)),
                    priority = groupTable.getField<long>(nameof(Group.priority))
                };
                // validate group
                if (!StringValidator.isIdentifier(cfgGroup.name)) {
                    throw new ConfigurationException(nameof(cfgGroup.name),
                        $"group name '{cfgGroup.name}' is not a valid identifier.");
                }
                // load permissions and rules
                cfgGroup.permissions = groupTable.getField<TomlArray>(nameof(Group.permissions))
                    .Select(x => new Permission(x.ToString()))
                    .ToList();
                var groupRules = groupTable.getTable(nameof(Group.rules));
                foreach (var appRulesTablePair in groupRules) {
                    var ruleApp = appRulesTablePair.Key;
                    var rulesTable = (TomlTable) appRulesTablePair.Value;
                    foreach (var appRule in rulesTable) {
                        cfgGroup.rules.Add(new AccessRule(ruleApp, appRule.Key, appRule.Value.ToString()));
                    }
                }

                groups.Add(cfgGroup);
            }

            var usersTable = tb.getTable(nameof(users));
            usersTable.autoBind(users);

            var loggingTable = tb.getTable(nameof(logging));
            loggingTable.autoBind(logging);
        }

        #endregion
    }
}
