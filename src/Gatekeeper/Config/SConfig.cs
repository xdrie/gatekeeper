#region

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

            /// <summary>
            /// Enables production mode (disables using hardcoded/test values)
            /// </summary>
            public bool production = true;
        }

        public Server server = new Server();

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