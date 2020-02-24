using Carter.Request;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Modules.Provider {
    public class AppAuthenticationModule : AuthenticatedUserModule {
        public AppAuthenticationModule(SContext serverContext) : base(AccessScope.rootScope, "/app", serverContext) {
            Get("/token/{appId}", async (req, res) => {
                var appId = req.RouteValues.As<string>("appId");
                // check config for app layers
            });
        }
    }
}