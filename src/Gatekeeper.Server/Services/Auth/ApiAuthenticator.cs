using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Models;

namespace Gatekeeper.Server.Services.Auth {
    public class ApiAuthenticator : DependencyObject {
        public ApiAuthenticator(SContext context) : base(context) { }

        public const string APP_SECRET_HEADER = "X-App-Secret";

        public const string CLAIM_USERNAME = "username";
        public const string CLAIM_TOKEN = "token";

        public ClaimsPrincipal resolveIdentity(string tokenStr) {
            var maybeCred = serverContext.tokenAuthenticator.resolve(tokenStr);
            if (maybeCred == null) return null;
            var cred = maybeCred.Value;

            var claims = new[] {
                new Claim(CLAIM_TOKEN, cred.token.content),
                new Claim(CLAIM_USERNAME, cred.token.user.username)
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}