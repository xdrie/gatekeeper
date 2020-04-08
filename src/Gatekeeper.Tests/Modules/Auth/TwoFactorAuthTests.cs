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
        private TotpSetupResponse totpSetup;
        public TwoFactorAuthTests(ServerTestFixture fx) : base(fx) { }

        public override async Task InitializeAsync() {
            await base.InitializeAsync();
            var resp = await client.GetAsync("/a/auth/setup2fa");
            resp.EnsureSuccessStatusCode();
            totpSetup = JsonConvert.DeserializeObject<TotpSetupResponse>(await resp.Content.ReadAsStringAsync());
        }

        private async Task confirmTotpSetup() {
            var totpProvider = new TotpProvider(Convert.FromBase64String(totpSetup.secret));

            var resp = await client.PostAsJsonAsync("/a/auth/confirm2fa", new TwoFactorConfirmRequest {
                otpcode = totpProvider.getCode()
            });
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task canGetTwoFactorSetupSecret() {
            Assert.True(Convert.FromBase64String(totpSetup.secret).Length == TotpProvider.TOTP_SECRET_LENGTH);
        }

        [Fact]
        public async Task canConfirmTwoFactor() {
            await confirmTotpSetup();
            // check the server context
            Assert.True(fx.serverContext.userManager.findByUsername(username).totpEnabled);
        }

        [Fact]
        public async Task requiresOtpCodeWhenTwoFactorEnabled() {
            await confirmTotpSetup();
            // attempt a login, which should fail with FailedDependency (requires OTP)
            var resp = await client.PostAsJsonAsync("/a/auth/login", new LoginRequest {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD
            });
            Assert.Equal(HttpStatusCode.FailedDependency, resp.StatusCode);
        }

        [Fact]
        public async Task canLoginTwoFactor() {
            await confirmTotpSetup();
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