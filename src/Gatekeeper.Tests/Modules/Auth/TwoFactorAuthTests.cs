using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Gatekeeper.Server.Services.Auth;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Gatekeeper.Tests.Utilities;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Auth {
    public class TwoFactorAuthTests : UserDependentTests {
        public TwoFactorAuthTests(ServerTestFixture fx) : base(fx) { }

        public async Task<TotpSetupResponse> registerAndStartTotpSetup() {
            var resp = await client.GetAsync("/a/auth/setup2fa");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<TotpSetupResponse>(await resp.Content.ReadAsStringAsync());
            return data;
        }

        private async Task confirmTotpSetup(string b64Secret) {
            var totpProvider = new TotpProvider(Convert.FromBase64String(b64Secret));

            var resp = await client.PostAsJsonAsync("/a/auth/confirm2fa", new TwoFactorConfirmRequest {
                otpcode = totpProvider.getCode()
            });
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task canGetTwoFactorSetupSecret() {
            var totpSetup = await registerAndStartTotpSetup();
            Assert.True(Convert.FromBase64String(totpSetup.secret).Length == TotpProvider.TOTP_SECRET_LENGTH);
        }

        [Fact]
        public async Task canConfirmTwoFactor() {
            var totpSetup = await registerAndStartTotpSetup();
            // check the server context
            Assert.True(fx.serverContext.userManager.findByUsername(username).totpEnabled);
        }

        [Fact]
        public async Task requiresOtpCodeWhenTwoFactorEnabled() {
            var totpSetup = await registerAndStartTotpSetup();
            // attempt a login, which should fail with FailedDependency (requires OTP)
            var resp = await client.PostAsJsonAsync("/a/auth/login", new LoginRequest {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD
            });
            Assert.Equal(HttpStatusCode.FailedDependency, resp.StatusCode);
        }

        [Fact]
        public async Task canLoginTwoFactor() {
            var totpSetup = await registerAndStartTotpSetup();
            var totpProvider = new TotpProvider(Convert.FromBase64String(totpSetup.secret));
            // attempt a 2fa login
            var resp = await client.PostAsJsonAsync("/a/auth/login2fa", new LoginRequestTwoFactor {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD,
                otpcode = totpProvider.getCode()
            });
            resp.EnsureSuccessStatusCode();
        }
    }
}