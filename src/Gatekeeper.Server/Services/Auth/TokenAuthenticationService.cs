using System;
using System.Collections.Generic;
using System.Linq;
using Gatekeeper.Models.Identity;
using Gatekeeper.Server.Config;
using Hexagon.Models;
using Hexagon.Utilities;

namespace Gatekeeper.Server.Services.Auth {
    public class TokenAuthenticationService : DependencyService<SContext> {
        public TokenAuthenticationService(SContext context) : base(context) { }
        
        public const int TOKEN_LENGTH = 32;
        public static TimeSpan ROOT_TOKEN_LIFETIME = TimeSpan.FromDays(7);

        public Token issue(AccessScope scope, TimeSpan lifetime) {
            return new Token {
                content = StringUtils.secureRandomString(TOKEN_LENGTH),
                expires = DateTime.UtcNow.Add(lifetime),
                scope = scope.path
            };
        }

        public Token issueRoot() => issue(new AccessScope(AccessScope.ROOT_PATH), ROOT_TOKEN_LIFETIME);

        public Credential? resolve(string tokenStr) {
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
            return new Credential(token, AccessScope.parse(token.scope));
        }

        /// <summary>
        /// delete any and all expired tokens
        /// </summary>
        public void prune() {
            using (var db = serverContext.getDbContext()) {
                var expiredTokens = new List<Token>();
                foreach (var token in db.tokens) {
                    if (token.expired()) {
                        expiredTokens.Add(token);
                    }
                }
                db.tokens.RemoveRange(expiredTokens);
                db.SaveChanges();
            }
        }
    }
}