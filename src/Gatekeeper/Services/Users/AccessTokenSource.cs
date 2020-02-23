using System;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Models.Identity;
using Hexagon.Utilities;

namespace Gatekeeper.Services.Users {
    public class AccessTokenSource : DependencyObject {
        public AccessTokenSource(SContext context) : base(context) { }

        public const int TOKEN_LENGTH = 32;
        public static TimeSpan TOKEN_LIFETIME = TimeSpan.FromDays(7);

        public Token issue(User user, string scope) {
            // TODO: make expiry time configurable
            return new Token {
                content = StringUtils.secureRandomString(TOKEN_LENGTH),
                user = user,
                expires = DateTime.Now.Add(TOKEN_LIFETIME),
                scope = scope
            };
        }

        public Token issueRoot(User user) => issue(user, "/");
    }
}