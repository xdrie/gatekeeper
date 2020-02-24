using System.Collections.Generic;
using Gatekeeper.Config;

namespace Gatekeeper.Models.Remote {
    public class GlobalRemoteApp : SConfig.RemoteApp {
        public override string name => "global";
        public override List<string> paths => new List<string> { "*" };
    }
}