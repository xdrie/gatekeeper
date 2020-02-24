using System.Net;
using Carter.ModelBinding;
using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Gatekeeper.OpenApi.Auth;
using Gatekeeper.Services.Users;
using Hexagon.Services.Application;
using Hexagon.Services.Serialization;

namespace Gatekeeper.Modules.Auth {
    public class AuthModule : ApiModule {
        public AuthModule(SContext context) : base("/auth", context) {
            Post<CreateUser>("/create", async (req, res) => {
                var createReq = await req.BindAndValidate<CreateUserRequest>();
                if (!createReq.ValidationResult.IsValid) {
                    res.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    await res.Negotiate(createReq.ValidationResult.GetFormattedErrors());
                    return;
                }

                // attempt to register user
                try {
                    // register the user
                    var user = serverContext.userManager.registerUser(createReq.Data);
                    serverContext.log.writeLine($"registered user {user.username}",
                        SLogger.LogLevel.Information);
                    var token = serverContext.userManager.issueRootToken(user.dbid);

                    // Return user details
                    res.StatusCode = (int) HttpStatusCode.Created;
                    await res.respondSerialized(new AuthedUserResponse {
                        user = new AuthenticatedUser(user),
                        token = token
                    });
                }
                catch (UserManagerService.UserAlreadyExistsException) {
                    res.StatusCode = (int) HttpStatusCode.Conflict;
                    return;
                }
            });

            Post<LoginUser>("/login", async (req, res) => {
                var loginReq = await req.BindAndValidate<LoginUserRequest>();
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
                    // check two-factor
                    if (user.totpEnabled) {
                        // require login with otp code, but creds were good
                        res.StatusCode = (int) HttpStatusCode.FailedDependency;
                        return;
                    }
                    
                    // issue a new token
                    var token = serverContext.userManager.issueRootToken(user.dbid);

                    // return user details
                    res.StatusCode = (int) HttpStatusCode.OK;
                    await res.respondSerialized(new AuthedUserResponse {
                        user = new AuthenticatedUser(user),
                        token = token
                    });
                    return;
                }

                res.StatusCode = (int) HttpStatusCode.Unauthorized;
                return;
            });

            Post<DeleteUser>("/delete", async (req, res) => {
                var loginReq = await req.BindAndValidate<LoginUserRequest>();
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
                    // check two-factor
                    if (user.totpEnabled) {
                        // require login with otp code, but creds were good
                        res.StatusCode = (int) HttpStatusCode.FailedDependency;
                        return;
                    }
                    
                    // delete the account
                    serverContext.userManager.deleteUser(user.dbid);

                    // return success indication
                    res.StatusCode = (int) HttpStatusCode.NoContent;
                    return;
                }

                res.StatusCode = (int) HttpStatusCode.Unauthorized;
                return;
            });
        }
    }
}