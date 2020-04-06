using FrenchFry.Demo.Services;
using Gatekeeper.Remote;
using Hexagon;
using Hexagon.Services;

namespace FrenchFry.Demo.Config {
    public class SContext : ServerContext {
        public const string GATE_SECRET = "yeet";
        public override IApiAuthenticator getAuthenticator() => new ApiAuthenticator(this);

        public GateAuthClient gateAuthClient { get; } = new GateAuthClient("FrenchFry", GATE_SECRET);
    }
}