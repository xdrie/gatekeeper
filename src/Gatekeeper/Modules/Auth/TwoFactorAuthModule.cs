using System;
using Gatekeeper.Config;
using Gatekeeper.OpenApi.Auth;

namespace Gatekeeper.Modules.Auth {
    public class TwoFactorAuthModule : ApiModule {
        public TwoFactorAuthModule(SContext serverContext) : base("/auth", serverContext) {
            Get<SetupTwoFactor>("/setup2fa", async (req, res) => {
                throw new NotImplementedException();
            });
        }
    }
}