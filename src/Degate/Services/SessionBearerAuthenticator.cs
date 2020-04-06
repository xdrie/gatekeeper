using System.Security.Claims;
using Degate.Config;
using Hexagon;
using Hexagon.Models;
using Hexagon.Services;

namespace Degate.Services {
    public class SessionBearerAuthenticator<TContext> : DependencyService<TContext>, IBearerAuthenticator
        where TContext : ServerContext, IDegateContext {
        public SessionBearerAuthenticator(TContext context) : base(context) { }

        public ClaimsPrincipal resolve(string token) {
            // we only accept session tokens here.
            // match the token to an existing session
            var auth = serverContext.sessionTokenResolver.resolve(token);
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