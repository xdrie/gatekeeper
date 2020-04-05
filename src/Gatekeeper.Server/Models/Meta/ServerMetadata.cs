using Gatekeeper.Server.Config;

namespace Gatekeeper.Server.Models.Meta {
    public class ServerMetadata {
        public string name { get; set; } = SConfig.SERVER_NAME;
        public string version { get; set; } = SConfig.VERSION;
    }
}