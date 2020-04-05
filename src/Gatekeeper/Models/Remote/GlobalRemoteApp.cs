using System.Collections.Generic;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Models.Remote {
    public class GlobalRemoteApp : RemoteApp {
        public const string GLOBAL_APP = "Global";
        public const string GLOBAL_SECRET = "secret";
        
        public override string name => GLOBAL_APP;
        public override List<string> layers => new List<string> { AccessScope.WILDCARD_PATH };
        public override string secret => GLOBAL_SECRET;
    }
}