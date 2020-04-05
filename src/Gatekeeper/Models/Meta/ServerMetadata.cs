using Gatekeeper.Config;

namespace Gatekeeper.Models.Meta {
    public class ServerMetadata {
        public string name { get; set; } = SConfig.SERVER_NAME;
        public string version { get; set; } = SConfig.VERSION;
    }
}