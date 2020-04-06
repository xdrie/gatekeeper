using FrenchFry.Demo.Config;
using Gatekeeper.Remote;
using Hexagon.Models;

namespace FrenchFry.Demo.Services {
    public class TokenResolverService : DependencyService<SContext> {
        public TokenResolverService(SContext context) : base(context) { }

        public GateUser resolve(string token) {
            // resolve user by token

            // 1. get matching session
            if (!serverContext.sessions.exists(token)) return null;
            
            var sess = serverContext.sessions.get(token);
            var user = sess.jar.Resolve<GateUser>();
            return user;
        }
    }
}