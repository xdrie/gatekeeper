using Gatekeeper.Config;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Modules.Provider {
    public class AppAuthenticationModule : AuthenticatedUserModule {
        public AppAuthenticationModule(SContext serverContext) : base(AccessScope.rootScope, "/app", serverContext) { }
    }
}