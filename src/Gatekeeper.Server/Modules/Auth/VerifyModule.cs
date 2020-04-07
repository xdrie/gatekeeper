using System.Net;
using Carter.Request;
using Gatekeeper.Models.Identity;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.OpenApi.Auth;
using Hexagon.Modules;

namespace Gatekeeper.Server.Modules.Auth {
    public class VerifyModule : ApiModule<SContext> {
        public VerifyModule(SContext serverContext) : base("/auth", serverContext) {
            Post<VerifyUser>("/verify/{uuid}/{code}", async (req, res) => {
                // make sure user exists
                var user = serverContext.userManager.findByUuid(req.RouteValues.As<string>("uuid"));
                if (user == null) {
                    res.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }

                // check if given code matches verification, then update role
                var code = req.RouteValues.As<string>("code");
                if (user.verification == code) {
                    user.role = User.Role.User; // promote user
                    serverContext.userManager.updateUser(user);
                    res.StatusCode = (int) HttpStatusCode.NoContent;
                    return;
                }

                res.StatusCode = (int) HttpStatusCode.Unauthorized;
                return;
            });
        }
    }
}