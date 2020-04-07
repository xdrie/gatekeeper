using Degate.Modules;
using FrenchFry.Demo.Config;
using Gatekeeper.Models.Identity;

namespace FrenchFry.Demo.Modules {
    public class BridgeLinkModule : BridgeLinkModule<SContext> {
        public BridgeLinkModule(SContext serverContext) : base("/bridge", serverContext) { }

        public override void updateRecord(RemoteAuthentication remoteAuth) {
            // create necessary records
            if (serverContext.userManager.findByUid(remoteAuth.user.uuid) == null) {
                serverContext.userManager.register(remoteAuth.user);
            }
        }
    }

    public class BridgeUserInfoModule : BridgeUserInfoModule<SContext> {
        public BridgeUserInfoModule(SContext serverContext) : base("/me", serverContext) { }
    }
}