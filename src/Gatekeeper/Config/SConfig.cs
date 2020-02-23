#region

using Hexagon.Services.Application;

#endregion

namespace Gatekeeper.Config {
    public class SConfig {
        public class Server {
            /// <summary>
            /// Database connection path.
            /// Specify a file using "Data Source=database.db"
            /// Specify in-memory transient using null
            /// </summary>
            // public string database = "Data Source=database.db";

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