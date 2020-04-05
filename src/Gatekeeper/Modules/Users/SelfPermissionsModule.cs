using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Access;
using Gatekeeper.OpenApi.Users;

namespace Gatekeeper.Modules.Users {
    public class SelfPermissionsModule : AuthenticatedUserModule {
        public SelfPermissionsModule(SContext serverContext) : base("/perms", serverContext) {
            Get<GetMyPerms>("/", async (req, res) => {
                // load user permissions
                var user = serverContext.userManager.loadPermissions(currentUser);
                await res.Negotiate(new PermissionList(user.permissions));
            });
        }
    }
}