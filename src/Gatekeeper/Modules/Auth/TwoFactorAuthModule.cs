using System;
using System.Net;
using Gatekeeper.Config;
using Gatekeeper.Models.Responses;
using Gatekeeper.OpenApi.Auth;
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
                var seed = StringUtils.secureRandomString(32);
                var seedBytes = Hasher.sha256(seed);
                currentUser.totp = seedBytes;
                
                // return the seed
                await res.respondSerialized(new TotpSetupResponse {
                    secret = Convert.ToBase64String(seedBytes)
                });
                return;
            });
        }
    }
}