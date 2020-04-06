using System.Security.Claims;
using FrenchFry.Demo.Config;
using Hexagon.Models;
using Hexagon.Services;

namespace FrenchFry.Demo.Services {
    public class BearerAuthenticator : DependencyService<SContext>, IBearerAuthenticator {
        public BearerAuthenticator(SContext context) : base(context) { }

        public ClaimsPrincipal resolve(string token) {
            // we only accept session tokens here.
            // match the token to an existing session
            var user = serverContext.tokenResolver.resolve(token);
            if (user == null) return null;
            
            var claims = new[] {
                new Claim(IBearerAuthenticator.CLAIM_TOKEN, token),
                new Claim(IBearerAuthenticator.CLAIM_USERNAME, user.auth.user.username),
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}