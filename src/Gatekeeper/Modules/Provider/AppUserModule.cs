using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.OpenApi.App;

namespace Gatekeeper.Modules.Provider {
    public class AppUserModule : AuthenticatedUserModule {
        public AppUserModule(SContext serverContext) : base("/app", serverContext) {
            Get<AppGetUser>("/user", async (req, res) => {
                await res.Negotiate(new PublicUser(currentUser));
            });
        }
    }
}