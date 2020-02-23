#region

using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Carter;

#endregion

namespace Gatekeeper.Services.Auth.Security {
    public static class ApiAccessModuleSecurityExtensions {
        // private static readonly Claim _adminClaim =
        //     new Claim(ApiAuthenticator.AUTH_TYPE_CLAIM_KEY, AccessScope.Admin.ToString());
        //
        // private static readonly Claim _userClaim =
        //     new Claim(ApiAuthenticator.AUTH_TYPE_CLAIM_KEY, AccessScope.User.ToString());
        //
        // public static void requiresUserAuthentication(this CarterModule module) {
        //     injectAuthenticationHook(module, _userClaim);
        // }
        //
        // public static void requiresAdminAuthentication(this CarterModule module) {
        //     injectAuthenticationHook(module, _adminClaim);
        // }

        public static void injectAuthenticationHook(CarterModule module, Claim requiredClaim) {
            module.Before += ctx => {
                if (ctx.User != null && ctx.User.ensureClaim(requiredClaim)) {
                    return Task.FromResult(true);
                }

                ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                return Task.FromResult(false);
            };
        }

        /// <summary>
        /// Ensures that a ClaimsPrincipal posesses a claim by checking the Type and Value fields
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public static bool ensureClaim(this ClaimsPrincipal principal, Claim claim) {
            return principal.HasClaim(claim.Type, claim.Value);
        }
    }
}