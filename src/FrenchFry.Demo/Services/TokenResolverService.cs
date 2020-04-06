using FrenchFry.Demo.Config;
using Gatekeeper.Remote;
using Hexagon.Models;

namespace FrenchFry.Demo.Services {
    public class TokenResolverService : DependencyService<SContext> {
        public TokenResolverService(SContext context) : base(context) { }

        public GateUser resolve(string token) {
            // resolve user by token
            return null;
        }
    }
}