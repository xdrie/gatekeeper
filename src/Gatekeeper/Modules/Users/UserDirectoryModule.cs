using System.Net;
using Carter.Request;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.OpenApi.Users;
using Hexagon.Services.Serialization;

namespace Gatekeeper.Modules.Users {
    public class UserDirectoryModule : ApiModule {
        public UserDirectoryModule(SContext serverContext) : base("/u", serverContext) {
            Get<GetPublicUser>("/{username}", async (req, res) => {
                var user = serverContext.userManager.findByUsername(req.RouteValues.As<string>("username"));
                if (user == null) {
                    res.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }

                var publicProfile = new PublicUser(user);
                await res.respondSerialized(publicProfile);
            });
        }
    }
}