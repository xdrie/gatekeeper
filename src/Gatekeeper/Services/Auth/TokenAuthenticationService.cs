using System;
using System.Linq;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Models.Identity;
using Hexagon.Utilities;

namespace Gatekeeper.Services.Auth {
    public class TokenAuthenticationService : DependencyObject {
        public TokenAuthenticationService(SContext context) : base(context) { }
        
        public const int TOKEN_LENGTH = 32;
        public static TimeSpan ROOT_TOKEN_LIFETIME = TimeSpan.FromDays(7);

        public Token issue(string scope, TimeSpan lifetime) {
            return new Token {
                content = StringUtils.secureRandomString(TOKEN_LENGTH),
                expires = DateTime.Now.Add(lifetime),
                scope = new AccessScope(AccessScope.ROOT_PATH).path
            };
        }

        public Token issueRoot() => issue("/", ROOT_TOKEN_LIFETIME);

        public TokenCredential? resolve(string tokenStr) {
            // 1. match the token string to a Token
            var token = default(Token);
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

            // 3. parse token scopes/path
            return new TokenCredential(token, AccessScope.parse(token.scope));
        }
    }
}