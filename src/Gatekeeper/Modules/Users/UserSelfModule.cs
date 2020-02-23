using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Modules.Users {
    public class UserSelfModule : AuthenticatedModule {
        public UserSelfModule(SContext serverContext) : base("/u", serverContext) {
            Get("/me", async (req, res) => { await res.Negotiate(new AuthenticatedUser(currentUser)); });
        }
    }
}