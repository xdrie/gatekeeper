using System.Collections.Generic;
using Carter.Response;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Models.Access;
using Gatekeeper.Server.Models.Identity;
using Gatekeeper.Server.OpenApi.Remote;
using Gatekeeper.Server.Services.Users;

namespace Gatekeeper.Server.Modules.Remote {
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