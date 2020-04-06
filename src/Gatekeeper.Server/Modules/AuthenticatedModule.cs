using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Remote;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Services.Auth;
using Gatekeeper.Server.Services.Auth.Security;
using Hexagon.Modules;

namespace Gatekeeper.Server.Modules {
    public abstract class AuthenticatedModule : ApiModule<SContext> {
        public User currentUser { get; private set; }
        public Credential credential { get; private set; }

        protected AuthenticatedModule(AccessScope minimumScope, User.Role minimumRole, string path,
            SContext serverContext) : base(path, serverContext) {
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
    /// Represents a module available to unverified/pending users
    /// </summary>
    public abstract class UnverifiedUserModule : AuthenticatedModule {
        protected UnverifiedUserModule(string path, SContext serverContext) : base(AccessScope.rootScope,
            User.Role.Pending, path, serverContext) { }
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

    /// <summary>
    /// Represents a module only available to application tokens
    /// </summary>
    public abstract class RemoteApplicationModule : AuthenticatedModule {
        public RemoteApp remoteApp { get; private set; }
        
        protected RemoteApplicationModule(string path, SContext serverContext) : base(AccessScope.globalScope,
            User.Role.User, path, serverContext) {
            this.Before += async (ctx) => {
                // verify the app secret
                var appSecret = ctx.Request.Headers[ApiAuthenticator.APP_SECRET_HEADER];
                if (string.IsNullOrEmpty(appSecret)) {
                    ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return false;
                }
                
                // match the token to an app
                remoteApp = serverContext.config.apps.SingleOrDefault(x => x.name == credential.scope.app);
                if (remoteApp == null || remoteApp.secret != appSecret) {
                    ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return false;
                }
                
                return true;
            };
        }
    }
}