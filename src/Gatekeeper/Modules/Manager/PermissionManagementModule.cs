using System;
using System.Net;
using Gatekeeper.Config;
using Gatekeeper.Models.Access;
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

                // fetch the user and update their permissions
                var user = serverContext.userManager.findByUuid(updateReq.userUuid);
                if (user == null) {
                    res.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }
                var updateType =
                    Enum.Parse<Permission.PermissionUpdateType>(updateReq.type, true);
                foreach (var permission in updateReq.permissions) {
                    serverContext.userManager.updatePermission(user.dbid, new Permission(permission), updateType);
                }
            });
        }
    }
}