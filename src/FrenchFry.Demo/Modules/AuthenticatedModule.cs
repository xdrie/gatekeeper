using System.Linq;
using FrenchFry.Demo.Config;
using Gatekeeper.Remote;
using Hexagon.Modules;
using Hexagon.Security;
using Hexagon.Services;

namespace FrenchFry.Demo.Modules {
    public abstract class AuthenticatedModule : ApiModule<SContext> {
        public GateUser currentUser { get; private set; }

        protected AuthenticatedModule(string path, SContext serverContext) : base(path, serverContext) {
            // require authentication
            this.requiresUserAuthentication();

            this.Before += async (ctx) => {
                // get the user
                var tokenClaim = ctx.User.Claims.First(x => x.Type == IBearerAuthenticator.CLAIM_TOKEN);
                currentUser = serverContext.tokenResolver.resolve(tokenClaim.Value);

                return true;
            };
        }
    }
}