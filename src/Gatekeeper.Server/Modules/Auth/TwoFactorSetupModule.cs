using System;
using System.Net;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.OpenApi.Auth;
using Gatekeeper.Server.Services.Auth;
using Hexagon.Serialization;
using Hexagon.Utilities;
using Hexagon.Web;

namespace Gatekeeper.Server.Modules.Auth {
    public class TwoFactorSetupModule : AuthenticatedUserModule {
        public TwoFactorSetupModule(SContext serverContext) : base("/auth", serverContext) {
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
                var validatedReq = await this.validateRequest<TwoFactorConfirmRequest>(req, res);
                if (!validatedReq.isValid) return;
                var confirmReq = validatedReq.request;

                // use TOTP provider to check code
                var provider = new TotpProvider(currentUser.totp);
                if (provider.verify(confirmReq.otpcode)) {
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