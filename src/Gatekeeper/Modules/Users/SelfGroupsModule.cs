using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.OpenApi.Users;

namespace Gatekeeper.Modules.Users {
    public class SelfGroupsModule : AuthenticatedUserModule {
        public SelfGroupsModule(SContext serverContext) : base("/perms", serverContext) {
            Get<GetMyGroups>("/", async (req, res) => {
                // load user permissions
                var user = serverContext.userManager.loadGroups(currentUser);
                await res.Negotiate(user.groups);
            });
        }
    }
}