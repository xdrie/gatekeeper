#region

using System.Collections.Generic;
using Gatekeeper.Models.Remote;
using Hexagon.Services.Application;

#endregion

namespace Gatekeeper.Config {
    public class SConfig {
        public const string SERVER_NAME = "ALTiCU Gatekeeper";
        public const string VERSION = "v0.0.4.2300-dev";

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

        public class RemoteApp {
            public virtual string name { get; set; }
            public virtual List<string> scopes { get; set; }
        }

        public List<RemoteApp> apps = new List<RemoteApp> {new GlobalRemoteApp()};

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