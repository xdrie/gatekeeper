using System;
using System.Linq;
using System.Security.Claims;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Services.Auth {
    public class ApiAuthenticator : DependencyObject {
        public ApiAuthenticator(SContext context) : base(context) { }

        public const string CLAIM_USERNAME = "username";

        public ClaimsPrincipal resolveIdentity(string tokenStr) {
            // 1. match the token string to a Token
            Token? token = default(Token);
            using (var db = serverContext.getDbContext()) {
                token = db.tokens.FirstOrDefault(x => x.content == tokenStr);
            }

            if (token == null) return null;

            // 2. check token validity
            if (DateTime.Now >= token.expires) {
                // token is expired.
                return null;
            }

            // 2. match the token to a user
            using (var db = serverContext.getDbContext()) {
                token = db.tokens.Find(token.dbid);
                db.Entry(token).Reference(x => x.user).Load();
            }

            var claims = new[] {
                new Claim(CLAIM_USERNAME, token.user.username)
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}