using Carter.Response;
using Gatekeeper.Models.Identity;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.OpenApi.Users;
using Gatekeeper.Server.Services.Users;

namespace Gatekeeper.Server.Modules.Users {
    public class SelfModule : AuthenticatedUserModule {
        public SelfModule(SContext serverContext) : base("/u", serverContext) {
            Get<GetMyself>("/me", async (req, res) => { await res.Negotiate(new AuthenticatedUser(currentUser)); });
            Get<GetMyGroups>("/groups", async (req, res) => {
                var user = serverContext.userManager.loadGroups(currentUser);
                await res.Negotiate(user.groups);
            });
            Get<GetMyRules>("/rules", async (req, res) => {
                var resolver = new PermissionResolver(serverContext, currentUser);
                var rules = resolver.aggregateRules();
                await res.Negotiate(rules);
            });
        }
    }
}