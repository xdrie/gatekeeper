using System;
using System.Net;
using Carter.ModelBinding;
using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Gatekeeper.OpenApi.Auth;
using Gatekeeper.Services.Auth;
using Hexagon.Services.Serialization;
using Hexagon.Utilities;

namespace Gatekeeper.Modules.Auth {
    public class TwoFactorAuthModule : AuthenticatedUserModule {
        public TwoFactorAuthModule(SContext serverContext) : base("/auth", serverContext) {
            Get<SetupTwoFactor>("/setup2fa", async (req, res) => {
                // check if two-factor ALREADY enabled
                if (currentUser.totpEnabled) {
                    // otp is already enabled
                    res.StatusCode = (int) HttpStatusCode.Conflict;
                    return;
                }

                // we generate a new TOTP secret
                var seed = StringUtils.secureRandomString(TotpProvider.TOTP_SECRET_LENGTH);
                var seedBytes = Hasher.sha256(seed);
                currentUser.totp = seedBytes;
                serverContext.userManager.updateUser(currentUser);

                // return the seed
                await res.respondSerialized(new TotpSetupResponse {
                    secret = Convert.ToBase64String(seedBytes)
                });
                return;
            });

            Post<ConfirmTwoFactor>("/confirm2fa", async (req, res) => {
                var confirmReq = await req.BindAndValidate<TwoFactorConfirmRequest>();
                if (!confirmReq.ValidationResult.IsValid) {
                    res.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    await res.Negotiate(confirmReq.ValidationResult.GetFormattedErrors());
                }

                // use TOTP provider to check code
                var provider = new TotpProvider(currentUser.totp);
                if (provider.verify(confirmReq.Data.otpcode)) {
                    // totp confirmed, enable totp and lock
                    serverContext.userManager.setupTotpLock(currentUser, credential.token);

                    res.StatusCode = (int) HttpStatusCode.OK;
                    return;
                }

                res.StatusCode = (int) HttpStatusCode.Unauthorized;
                return;
            });
        }
    }
}