using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.OpenApi.Remote;

namespace Gatekeeper.Modules.Remote {
    public class RemoteUserModule : RemoteApplicationModule {
        public RemoteUserModule(SContext serverContext) : base("/remote", serverContext) {
            Get<RemoteGetUser>("/user", async (req, res) => {
                await res.Negotiate(new PublicUser(currentUser));
            });
        }
    }
}