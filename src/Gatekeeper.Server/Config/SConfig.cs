#region

using System.Collections.Generic;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Remote;
using Hexagon.Logging;

#endregion

namespace Gatekeeper.Server.Config {
    public class SConfig {
        public const string SERVER_NAME = "ALTiCU Gatekeeper.Server";
        public const string VERSION = "0.0.9-dev";

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
    }
}