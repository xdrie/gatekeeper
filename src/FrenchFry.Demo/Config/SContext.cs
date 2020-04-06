using System;
using Degate.Config;
using Degate.Services;
using Gatekeeper.Remote;
using Hexagon;
using Hexagon.Services;

namespace FrenchFry.Demo.Config {
    public class SContext : ServerContext, IDegateContext {
        public const string GATE_SERVER = "http://localhost:5000";
        public const string GATE_SECRET = "yeet";

        public IRemoteTokenResolver sessionTokenResolver => new SessionTokenResolver<SContext>(this);
        public override IBearerAuthenticator getAuthenticator() => new SessionBearerAuthenticator<SContext>(this);

        public GateAuthClient gateAuthClient { get; } =
            new GateAuthClient("FrenchFry", new Uri(GATE_SERVER), GATE_SECRET);
    }
}