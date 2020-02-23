using Gatekeeper.Models.Identity;

namespace Gatekeeper.Models.Responses {
    public class CreatedUserResponse {
        public AuthenticatedUser user { get; set; }
        public Token token { get; set; }
    }
}