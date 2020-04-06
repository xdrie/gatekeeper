using Degate.Modules;
using FrenchFry.Demo.Config;

namespace FrenchFry.Demo.Modules {
    public class GateModule : GateLinkModule<SContext> {
        public GateModule(SContext serverContext) : base(serverContext) { }
    }
}