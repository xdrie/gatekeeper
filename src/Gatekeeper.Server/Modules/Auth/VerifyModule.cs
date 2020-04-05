using System.Net;
using Carter.Request;
using Gatekeeper.Models.Identity;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.OpenApi.Auth;

namespace Gatekeeper.Server.Modules.Auth {
    public class VerifyModule : UnverifiedUserModule {
        public VerifyModule(SContext serverContext) : base("/auth", serverContext) {
            Post<VerifyUser>("/verify/{code}", async (req, res) => {
                // check if given code matches verification, then update role
                var code = req.RouteValues.As<string>("code");
                if (currentUser.verification == code) {
                    currentUser.role = User.Role.User; // promote user
                    serverContext.userManager.updateUser(currentUser);
                    res.StatusCode = (int) HttpStatusCode.NoContent;
                    return;
                }

                res.StatusCode = (int) HttpStatusCode.Unauthorized;
                return;
            });
        }
    }
}