using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.OpenApi.Users;

namespace Gatekeeper.Modules.Users {
    public class UserSelfModule : AuthenticatedModule {
        public UserSelfModule(SContext serverContext) : base("/u", serverContext) {
            Get<GetMyself>("/me", async (req, res) => { await res.Negotiate(new AuthenticatedUser(currentUser)); });
        }
    }
}