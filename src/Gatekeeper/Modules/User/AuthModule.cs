using System.Net;
using System.Security;
using Carter.ModelBinding;
using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Gatekeeper.OpenApi;
using Gatekeeper.Services.Users;
using Hexagon.Services.Application;
using Hexagon.Services.Serialization;

namespace Gatekeeper.Modules.User {
    public class AuthModule : ApiModule {
        public AuthModule(SContext context) : base("/user", context) {
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
                    // register the user
                    var user = serverContext.userManager.registerUser(createReq.Data);
                    serverContext.log.writeLine($"registered user {user.username}",
                        SLogger.LogLevel.Information);

                    // Return user details
                    res.StatusCode = (int) HttpStatusCode.Created;
                    await res.respondSerialized(new CreatedUserResponse {
                        user = new AuthenticatedUser(user),
                        // token = 
                    });
                }
                catch (UserManagerService.UserAlreadyExistsException) {
                    res.StatusCode = (int) HttpStatusCode.Conflict;
                    return;
                }
            });

            Post<LoginUser>("/login", async (req, res) => {
                var loginReq = await req.BindAndValidate<UserLoginRequest>();
                if (!loginReq.ValidationResult.IsValid) {
                    res.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    await res.Negotiate(loginReq.ValidationResult.GetFormattedErrors());
                    return;
                }

                var user = serverContext.userManager.findByUsername(loginReq.Data.username);
                if (user == null) {
                    res.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return;
                }

                // validate password
                if (serverContext.userManager.checkPassword(loginReq.Data.password, user)) {
                    // var metrics = new UserMetricsService(serverContext);
                    // metrics.log(user.identifier, MetricsEventType.Auth);
                    // Return user details
                    res.StatusCode = (int) HttpStatusCode.OK;
                    await res.respondSerialized(new AuthenticatedUser(user));
                    return;
                }

                res.StatusCode = (int) HttpStatusCode.Unauthorized;
                return;
            });
        }
    }
}