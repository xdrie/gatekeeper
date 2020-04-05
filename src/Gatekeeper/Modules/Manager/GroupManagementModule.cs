using System;
using System.Linq;
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

                // ensure all the groups exist
                foreach (var group in updateReq.groups) {
                    if (serverContext.config.groups.All(x => x.name != group)) {
                        res.StatusCode = (int) HttpStatusCode.NotFound;
                        return;
                    }
                }

                // fetch the user
                var user = serverContext.userManager.findByUuid(updateReq.userUuid);
                if (user == null) {
                    res.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }

                // update their group memberships
                var updateType =
                    Enum.Parse<Group.UpdateType>(updateReq.type, true);
                foreach (var group in updateReq.groups) {
                    serverContext.userManager.updateGroupMembership(user.dbid, group, updateType);
                }
            });
        }
    }
}