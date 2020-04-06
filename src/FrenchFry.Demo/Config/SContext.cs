using System;
using FrenchFry.Demo.Services;
using Gatekeeper.Remote;
using Hexagon;
using Hexagon.Services;

namespace FrenchFry.Demo.Config {
    public class SContext : ServerContext {
        public const string GATE_SERVER = "http://localhost:5000";
        public const string GATE_SECRET = "yeet";
        public override IApiAuthenticator getAuthenticator() => new ApiAuthenticator(this);

        public GateAuthClient gateAuthClient { get; } =
            new GateAuthClient("FrenchFry", new Uri(GATE_SERVER), GATE_SECRET);
    }
}