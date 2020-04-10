using System.Net;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.OpenApi.Auth;
using Gatekeeper.Server.Services.Auth;
using Gatekeeper.Server.Services.Users;
using Hexagon.Modules;
using Hexagon.Serialization;
using Hexagon.Web;
using Iri.Glass.Logging;
using Microsoft.AspNetCore.Http;

namespace Gatekeeper.Server.Modules.Auth {
    public class AuthModule : ApiModule<SContext> {
        public class ValidatedLogin<TLoginRequest> : ValidatedRequest<TLoginRequest>
            where TLoginRequest : LoginRequest {
            public User user;

            public ValidatedLogin(TLoginRequest request, bool isValid, User user = null) : base(request, isValid) {
                this.user = user;
            }

            public new static ValidatedLogin<TLoginRequest> failure() => new ValidatedLogin<TLoginRequest>(null, false);
        }

        public async Task<ValidatedLogin<TLoginRequest>> validateAndCheckPassword<TLoginRequest>(HttpRequest req,
            HttpResponse res)
            where TLoginRequest : LoginRequest {
            // validate model
            var validatedLoginReq = await this.validateRequest<TLoginRequest>(req, res);
            if (!validatedLoginReq.isValid) {
                return ValidatedLogin<TLoginRequest>.failure();
            }

            var loginReq = validatedLoginReq.request;

            // make sure user exists
            var user = serverContext.userManager.findByUsername(loginReq.username);
            if (user == null) {
                res.StatusCode = (int) HttpStatusCode.Unauthorized;
                return ValidatedLogin<TLoginRequest>.failure();
            }

            // validate password
            if (!serverContext.userManager.checkPassword(loginReq.password, user)) {
                res.StatusCode = (int) HttpStatusCode.Unauthorized;
                return ValidatedLogin<TLoginRequest>.failure();
            }

            return new ValidatedLogin<TLoginRequest>(loginReq, true, user);
        }

        public AuthModule(SContext context) : base("/auth", context) {
            Post<CreateUser>("/create", async (req, res) => {
                var validatedCreateReq = await this.validateRequest<RegisterRequest>(req, res);
                if (!validatedCreateReq.isValid) return;
                var createReq = validatedCreateReq.request;

                // attempt to register user
                try {
                    // register the user
                    var user = serverContext.userManager.registerUser(createReq);
                    serverContext.log.writeLine($"registered user {user.username}",
                        Logger.Verbosity.Information);
                    var token = serverContext.userManager.issueRootToken(user.id);

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
                var login = await validateAndCheckPassword<LoginRequest>(req, res);
                if (login.isValid) {
                    // check two-factor
                    if (login.user.totpEnabled) {
                        // require login with otp code, but creds were good
                        res.StatusCode = (int) HttpStatusCode.FailedDependency;
                        return;
                    }

                    // issue a new token
                    var token = serverContext.userManager.issueRootToken(login.user.id);

                    // return user details
                    res.StatusCode = (int) HttpStatusCode.OK;
                    await res.respondSerialized(new AuthedUserResponse {
                        user = new AuthenticatedUser(login.user),
                        token = token
                    });
                }
            });

            Post<LoginTwoFactor>("/login2fa", async (req, res) => {
                var login = await validateAndCheckPassword<LoginRequestTwoFactor>(req, res);
                if (login.isValid) {
                    // this route is not to be used unless two factor is enabled
                    if (!login.user.totpEnabled) {
                        res.StatusCode = (int) HttpStatusCode.PreconditionFailed;
                        return;
                    }
                    
                    // use TOTP provider to check code
                    var provider = new TotpProvider(login.user.totp);
                    if (!provider.verify(login.request.otpcode)) {
                        res.StatusCode = (int) HttpStatusCode.Unauthorized;
                        return;
                    }

                    // issue a new token
                    var token = serverContext.userManager.issueRootToken(login.user.id);

                    // return user details
                    res.StatusCode = (int) HttpStatusCode.OK;
                    await res.respondSerialized(new AuthedUserResponse {
                        user = new AuthenticatedUser(login.user),
                        token = token
                    });
                }
            });

            Post<DeleteUser>("/delete", async (req, res) => {
                var login = await validateAndCheckPassword<LoginRequest>(req, res);
                if (login.isValid) {
                    // delete the account
                    serverContext.userManager.deleteUser(login.user.id);
                    // return success indication
                    res.StatusCode = (int) HttpStatusCode.NoContent;
                    return;
                }
            });
        }
    }
}