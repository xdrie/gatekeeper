using FrenchFry.Demo.Services;
using Hexagon;
using Hexagon.Services;

namespace FrenchFry.Demo.Config {
    public class SContext : ServerContext {
        public const string GATE_SECRET = "yeet";
        public override IApiAuthenticator getAuthenticator() => new ApiAuthenticator(this);
    }
}