using System;
using Degate.Config;
using Degate.Services;
using Gatekeeper.Remote;
using Hexagon;
using Hexagon.Services;

namespace FrenchFry.Demo.Config {
    public class SContext : ServerContext, IDegateContext {
        public const string GATE_APP = "FrenchFry";
        public const string GATE_SERVER = "http://localhost:5000";
        public const string GATE_SECRET = "yeet";
        
        public override IBearerAuthenticator getAuthenticator() => new BearerAuthenticator<SContext>(this);
        
        public ISessionResolver sessionTokenResolver { get; }
        public GateAuthClient gateAuthClient { get; }

        public SContext() {
            sessionTokenResolver = new SessionResolver<SContext>(this);
            gateAuthClient = new GateAuthClient(GATE_APP, new Uri(GATE_SERVER), GATE_SECRET);
        }
    }
}