#region

using System;
using System.Collections.Generic;
using System.Linq;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Remote;

#endregion

namespace Gatekeeper.Server.Config {
    public class SConfig {
        public const string BRAND = "GaTE";
        public const string SERVER_NAME = "ALTiCU Gatekeeper.Server";
        public const string VERSION = "0.2.4";

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
            /// Whether to enable ASP.NET Core verbose logging
            /// </summary>
            public bool aspnetVerboseLogging = true;

            /// <summary>
            /// Whether to enable detailed/sensitive database logging
            /// </summary>
            public bool databaseLogging = true;
        }

        public Logging logging = new Logging();
    }
}