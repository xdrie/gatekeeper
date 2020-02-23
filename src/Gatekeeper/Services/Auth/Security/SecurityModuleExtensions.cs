#region

using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Carter;

#endregion

namespace Gatekeeper.Services.Auth.Security {
    public static class ApiAccessModuleSecurityExtensions {
        public static void requiresUserAuthentication(this CarterModule module) {
            authenticationHook(module, ApiAuthenticator.CLAIM_USERNAME);
        }

        public static void authenticationHook(CarterModule module, string claimType) {
            module.Before += ctx => {
                if (ctx.User != null && ctx.User.ensureClaimPresent(claimType)) {
                    return Task.FromResult(true);
                }

                ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                return Task.FromResult(false);
            };
        }

        public static bool ensureClaimPresent(this ClaimsPrincipal principal, string claimType) {
            return principal.HasClaim(x => x.Type == claimType);
        }
    }
}