using System.Net;
using Carter.ModelBinding;
using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Requests;

namespace Gatekeeper.Modules.Manager {
    public class PermissionManagementModule : AdminModule {
        public PermissionManagementModule(SContext serverContext) : base("/perms", serverContext) {
            Patch("/update", async (req, res) => {
                var updateReq = await req.BindAndValidate<UpdatePermissionRequest>();
                if (!updateReq.ValidationResult.IsValid) {
                    res.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    await res.Negotiate(updateReq.ValidationResult.GetFormattedErrors());
                }
                
                // TODO: fetch the user and update their permissions
            });
        }
    }
}