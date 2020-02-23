using System.Net;
using System.Security;
using Carter.ModelBinding;
using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Requests;
using Gatekeeper.OpenApi;
using Hexagon.Services.Application;
using Hexagon.Services.Serialization;

namespace Gatekeeper.Modules.User {
    public class RegistrationModule : ApiModule {
        public RegistrationModule(SContext context) : base("/user", context) {
            Post<CreateUser>("/create", async (req, res) => {
                var createReq = await req.BindAndValidate<UserCreateRequest>();
                if (!createReq.ValidationResult.IsValid) {
                    res.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    await res.Negotiate(createReq.ValidationResult.GetFormattedErrors());
                    return;
                }

                if (serverContext.config.server.maxUsers > -1 &&
                    serverContext.userManager.registeredUserCount >= serverContext.config.server.maxUsers) {
                    res.StatusCode = (int) HttpStatusCode.InsufficientStorage;
                    return;
                }

                // attempt to register user
                try {
                    var user = serverContext.userManager.registerUser(createReq.Data);
                    serverContext.log.writeLine($"registered user {user.username}",
                        SLogger.LogLevel.Information);

                    // Return user details
                    await res.respondSerialized(user);
                }
                catch (SecurityException) {
                    res.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return;
                }
            });
        }
    }
}