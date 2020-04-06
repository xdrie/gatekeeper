using System.Security.Claims;
using Gatekeeper.Server.Config;
using Hexagon.Models;
using Hexagon.Services;

namespace Gatekeeper.Server.Services.Auth {
    public class BearerAuthenticator : DependencyService<SContext>, IBearerAuthenticator {
        public BearerAuthenticator(SContext context) : base(context) { }

        public ClaimsPrincipal resolve(string token) {
            var maybeCred = serverContext.tokenResolver.resolve(token);
            if (maybeCred == null) return null;
            var cred = maybeCred.Value;

            var claims = new[] {
                new Claim(IBearerAuthenticator.CLAIM_TOKEN, cred.token.content),
                new Claim(IBearerAuthenticator.CLAIM_USERNAME, cred.token.user.username)
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}