using System.Collections.Generic;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Models.Remote {
    public class GlobalRemoteApp : SConfig.RemoteApp {
        public const string DEFAULT_PERMISSION = "/Default";
        public override string name => "Global";
        public override List<string> layers => new List<string> { AccessScope.WILDCARD_PATH, "Default" };
    }
}