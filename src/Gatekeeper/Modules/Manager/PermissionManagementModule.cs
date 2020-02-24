using Gatekeeper.Config;
using Gatekeeper.Models.Requests;
using Gatekeeper.OpenApi.Manager;
using Hexagon.Web;

namespace Gatekeeper.Modules.Manager {
    public class PermissionManagementModule : AdminModule {
        public PermissionManagementModule(SContext serverContext) : base("/perms", serverContext) {
            Patch<UpdatePerms>("/update", async (req, res) => {
                var validatedReq = await this.validateRequest<UpdatePermissionRequest>(req, res);
                if (!validatedReq.isValid) return;
                var updateReq = validatedReq.request;
                
                // TODO: fetch the user and update their permissions
            });
        }
    }
}