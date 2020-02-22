namespace Gatekeeper.Config {
    public class SContext {
        // Configuration parameters
        public SConfig config { get; }

        public SContext(SConfig config) {
            this.config = config;
        }
    }
}