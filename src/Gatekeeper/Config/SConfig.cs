#region

using System.Collections.Generic;

#endregion

namespace Gatekeeper.Config {
    public class SConfig {
        public class Server {
            /// <summary>
            /// Database connection path.
            /// Specify a file using "Data Source=database.db"
            /// Specify in-memory transient using null
            /// </summary>
            public string database = "Data Source=database.db";

            /// <summary>
            /// Maximum number of registered users. Set to -1 for unlimited.
            /// </summary>
            public long maxUsers = -1;
        }

        public Server server = new Server();

        public class Logging {
            /// <summary>
            /// The verbosity of the application logger
            /// </summary>
            public SpeercsLogger.LogLevel logLevel = SpeercsLogger.LogLevel.Information;

            /// <summary>
            /// Whether to enable ASP.NET Core verbose logging
            /// </summary>
            public bool aspnetVerboseLogging = true;

            /// <summary>
            /// Whether to enable detailed/sensitive database logging
            /// </summary>
            public bool databaseLogging = true;

            public MetricsLevel metricsLevel = MetricsLevel.Full;

            /// <summary>
            /// Whether to send script execution errors that occur inside the server code.
            /// This can potentially be a security risk.
            /// </summary>
            public bool sendScriptErrors = false;
        }

        public Logging logging = new Logging();
    }
}