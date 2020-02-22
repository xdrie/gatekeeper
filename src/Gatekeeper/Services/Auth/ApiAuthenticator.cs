using System.Security.Claims;
using Gatekeeper.Config;
using Gatekeeper.Models;

namespace Gatekeeper.Services.Auth {
    public class ApiAuthenticator : DependencyObject {
        public ApiAuthenticator(SContext context) : base(context) { }

        public ClaimsPrincipal resolveIdentity(string token) {
            return null;
            // throw new NotImplementedException();
        }
    }
}