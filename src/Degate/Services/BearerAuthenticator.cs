using System.Security.Claims;
using Degate.Config;
using Hexagon;
using Hexagon.Models;
using Hexagon.Services;

namespace Degate.Services {
    public class BearerAuthenticator<TContext> : DependencyService<TContext>, IBearerAuthenticator
        where TContext : ServerContext, IDegateContext {
        public BearerAuthenticator(TContext context) : base(context) { }

        public ClaimsPrincipal resolve(string token) {
            serverContext.tickService.tick();
            // we only accept session tokens here.
            // match the token to an existing session
            var auth = serverContext.sessionResolver.resolveSessionToken(token);
            if (auth == null) return null;

            var claims = new[] {
                new Claim(IBearerAuthenticator.CLAIM_TOKEN, token),
                new Claim(IBearerAuthenticator.CLAIM_USERNAME, auth.user.username),
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}