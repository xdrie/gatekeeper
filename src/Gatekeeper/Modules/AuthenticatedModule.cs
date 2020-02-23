using System.Linq;
using System.Threading.Tasks;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.Services.Auth;
using Gatekeeper.Services.Auth.Security;

namespace Gatekeeper.Modules {
    public abstract class AuthenticatedModule : ApiModule {
        public User currentUser { get; private set; }

        protected AuthenticatedModule(string path, SContext serverContext) : base(path, serverContext) {
            // require authentication
            this.requiresUserAuthentication();

            Before += context => {
                var usernameClaim = context.User.Claims.First(x => x.Type == ApiAuthenticator.CLAIM_USERNAME);
                currentUser = serverContext.userManager.findByUsername(usernameClaim.Value);

                return Task.FromResult(true);
            };
        }
    }
}