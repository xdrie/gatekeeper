using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.Services.Auth;
using Gatekeeper.Services.Auth.Security;

namespace Gatekeeper.Modules {
    public abstract class AuthenticatedModule : ApiModule {
        public User currentUser { get; private set; }
        public Credential credential { get; private set; }

        protected AuthenticatedModule(AccessScope minimumScope, User.Role minimumRole, string path,
            SContext serverContext) : base(path,
            serverContext) {
            // require authentication
            this.requiresUserAuthentication();

            this.Before += async (ctx) => {
                var usernameClaim = ctx.User.Claims.First(x => x.Type == ApiAuthenticator.CLAIM_USERNAME);
                currentUser = serverContext.userManager.findByUsername(usernameClaim.Value);

                if (currentUser.role < minimumRole) {
                    ctx.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                    return false;
                }

                var tokenClaim = ctx.User.Claims.First(x => x.Type == ApiAuthenticator.CLAIM_TOKEN);
                credential = serverContext.tokenAuthenticator.resolve(tokenClaim.Value).Value;

                // check if at least minimum scope
                if (!credential.scope.greaterThan(minimumScope)) {
                    ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return false;
                }

                return true;
            };
        }
    }

    /// <summary>
    /// Represents a module only available directly to an authenticated user
    /// </summary>
    public abstract class AuthenticatedUserModule : AuthenticatedModule {
        protected AuthenticatedUserModule(string path, SContext serverContext) : base(AccessScope.rootScope,
            User.Role.User, path, serverContext) { }
    }
    
    /// <summary>
    /// Represents a module only available to an admin
    /// </summary>
    public abstract class AdminModule : AuthenticatedModule {
        protected AdminModule(string path, SContext serverContext) : base(AccessScope.rootScope,
            User.Role.Admin, path, serverContext) { }
    }
}