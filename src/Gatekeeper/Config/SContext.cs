using Gatekeeper.Services.User;
using Hexagon.Services.Application;

namespace Gatekeeper.Config {
    public class SContext {
        /// <summary>
        /// configuration used to create this context instance
        /// </summary>
        public SConfig config { get; }
        
        public UserManagerService userManager { get; set; }

        public SLogger log;

        public SContext(SConfig config) {
            this.config = config;
            log = new SLogger(config.logging.logLevel);
        }
    }
}