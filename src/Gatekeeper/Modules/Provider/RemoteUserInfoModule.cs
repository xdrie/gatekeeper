using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.OpenApi.App;

namespace Gatekeeper.Modules.Provider {
    public class RemoteUserInfoModule : RemoteApplicationModule {
        public RemoteUserInfoModule(SContext serverContext) : base("/app", serverContext) {
            Get<RemoteGetUser>("/user", async (req, res) => {
                await res.Negotiate(new PublicUser(currentUser));
            });
        }
    }
}