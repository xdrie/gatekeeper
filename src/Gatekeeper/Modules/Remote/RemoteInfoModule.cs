using System.Collections.Generic;
using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Identity;
using Gatekeeper.OpenApi.Remote;
using Gatekeeper.Services.Users;

namespace Gatekeeper.Modules.Remote {
    public class RemoteInfoModule : RemoteApplicationModule {
        public RemoteInfoModule(SContext serverContext) : base("/remote", serverContext) {
            Get<RemoteGetInfo>("/",
                async (req, res) => { await res.Negotiate(new RemoteAuthentication(getUser(), getRules())); });
            Get<RemoteGetUser>("/user", async (req, res) => { await res.Negotiate(getUser()); });
        }

        private PublicUser getUser() => new PublicUser(currentUser);

        private IEnumerable<AccessRule> getRules() {
            var resolver = new PermissionResolver(serverContext, currentUser);
            return resolver.aggregateRulesForApp(remoteApp.name);
        }
    }
}