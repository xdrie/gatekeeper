using System;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Models.Identity;
using Hexagon.Utilities;

namespace Gatekeeper.Services.Users {
    public class AccessTokenSource : DependencyObject {
        public AccessTokenSource(SContext context) : base(context) { }

        public const int TOKEN_LENGTH = 32;
        public static TimeSpan ROOT_TOKEN_LIFETIME = TimeSpan.FromDays(7);

        public Token issue(string scope, TimeSpan lifetime) {
            // TODO: make expiry time configurable
            return new Token {
                content = StringUtils.secureRandomString(TOKEN_LENGTH),
                expires = DateTime.Now.Add(lifetime),
                scope = scope
            };
        }

        public Token issueRoot() => issue("/", ROOT_TOKEN_LIFETIME);
    }
}