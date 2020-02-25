using System;
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
                serverContext.userManager.loadPermissions(user);
                var updateType =
                    Enum.Parse<UpdatePermissionRequest.PermissionUpdateType>(updateReq.type);
                foreach (var permission in updateReq.permissions) {
                    switch (updateType) {
                        case UpdatePermissionRequest.PermissionUpdateType.Add:
                            user.permissions.Add(new Permission(permission));
                            break;
                        case UpdatePermissionRequest.PermissionUpdateType.Remove:
                            user.permissions.RemoveAll(x => x.path == permission);
                            break;
                    }
                }
                // save the user
                serverContext.userManager.updateUser(user);
            });
        }
    }
}