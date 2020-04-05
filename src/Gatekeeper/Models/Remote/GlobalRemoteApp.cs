using System.Collections.Generic;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Models.Remote {
    public class GlobalRemoteApp : SConfig.RemoteApp {
        public override string name => "Global";
        public override List<string> layers => new List<string> { AccessScope.WILDCARD_PATH };
        public override string secret => "secret";
    }
}