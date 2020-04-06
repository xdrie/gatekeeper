using Degate.Modules;
using FrenchFry.Demo.Config;

namespace FrenchFry.Demo.Modules {
    public class BridgeLinkModule : BridgeLinkModule<SContext> {
        public BridgeLinkModule(SContext serverContext) : base("/bridge", serverContext) { }
    }

    public class BridgeUserInfoModule : BridgeUserInfoModule<SContext> {
        public BridgeUserInfoModule(SContext serverContext) : base("/me", serverContext) { }
    }
}