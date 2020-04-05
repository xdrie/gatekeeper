using System.Net;
using Carter.Request;
using Gatekeeper.Models.Identity;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.OpenApi.Users;
using Hexagon.Serialization;

namespace Gatekeeper.Server.Modules.Users {
    public class UserDirectoryModule : ApiModule {
        public UserDirectoryModule(SContext serverContext) : base("/u", serverContext) {
            Get<GetPublicUser>("/{username}", async (req, res) => {
                var user = serverContext.userManager.findByUsername(req.RouteValues.As<string>("username"));
                if (user == null || user.role == User.Role.Pending) {
                    res.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }

                var publicProfile = new PublicUser(user);
                await res.respondSerialized(publicProfile);
            });
        }
    }
}