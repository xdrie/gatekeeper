using Gatekeeper.Server.Models.Identity;

namespace Gatekeeper.Server.Models.Responses {
    public class AuthedUserResponse {
        public AuthenticatedUser user { get; set; }
        public Token token { get; set; }
    }
}