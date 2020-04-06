using System.Linq;
using FrenchFry.Demo.Config;
using Gatekeeper.Models.Identity;
using Hexagon.Modules;
using Hexagon.Security;
using Hexagon.Services;

namespace FrenchFry.Demo.Modules {
    public abstract class AuthenticatedModule : ApiModule<SContext> {
        public RemoteAuthentication user { get; private set; }

        protected AuthenticatedModule(string path, SContext serverContext) : base(path, serverContext) {
            // require authentication
            this.requiresUserAuthentication();

            Before += async (ctx) => {
                // get the user
                var tokenClaim = ctx.User.Claims.First(x => x.Type == IBearerAuthenticator.CLAIM_TOKEN);
                user = serverContext.tokenResolver.resolve(tokenClaim.Value);

                return true;
            };
        }
    }
}