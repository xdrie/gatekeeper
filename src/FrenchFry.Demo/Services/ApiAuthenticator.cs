using System.Security.Claims;
using FrenchFry.Demo.Config;
using Hexagon.Models;
using Hexagon.Services;

namespace FrenchFry.Demo.Services {
    public class ApiAuthenticator : DependencyService<SContext>, IApiAuthenticator {
        public ApiAuthenticator(SContext context) : base(context) { }

        public ClaimsPrincipal resolve(string token) {
            // we only accept session tokens here.
            // match the token to an existing session
            return null;
        }
    }
}