using System;
using System.Net;
using Gatekeeper.Config;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Requests;
using Gatekeeper.OpenApi.Manager;
using Hexagon.Web;

namespace Gatekeeper.Modules.Manager {
    public class GroupManagementModule : AdminModule {
        public GroupManagementModule(SContext serverContext) : base("/groups", serverContext) {
            Patch<UpdateGroups>("/update", async (req, res) => {
                var validatedReq = await this.validateRequest<UpdateGroupRequest>(req, res);
                if (!validatedReq.isValid) return;
                var updateReq = validatedReq.request;

                // fetch the user and update their permissions
                var user = serverContext.userManager.findByUuid(updateReq.userUuid);
                if (user == null) {
                    res.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }

                var updateType =
                    Enum.Parse<Group.UpdateType>(updateReq.type, true);
                foreach (var group in updateReq.groups) {
                    serverContext.userManager.updateGroupMembership(user.dbid, group, updateType);
                }
            });
        }
    }
}