using System.Linq;
using System.Threading.Tasks;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.Services.Auth;
using Gatekeeper.Services.Auth.Security;

namespace Gatekeeper.Modules {
    public abstract class AuthenticatedUserModule : ApiModule {
        public User currentUser { get; private set; }
        public TokenCredential credential { get; private set; }

        protected AuthenticatedUserModule(string path, SContext serverContext) : base(path, serverContext) {
            // require authentication
            this.requiresUserAuthentication();

            this.Before += async (ctx) => {
                var usernameClaim = ctx.User.Claims.First(x => x.Type == ApiAuthenticator.CLAIM_USERNAME);
                currentUser = serverContext.userManager.findByUsername(usernameClaim.Value);
                
                var tokenClaim = ctx.User.Claims.First(x => x.Type == ApiAuthenticator.CLAIM_TOKEN);
                credential = serverContext.tokenAuthenticator.resolve(tokenClaim.Value).Value;

                return true;
            };
        }
    }
}