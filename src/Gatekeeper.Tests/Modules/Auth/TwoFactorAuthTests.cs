using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Gatekeeper.Services.Auth;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Gatekeeper.Tests.Utilities;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Auth {
    [Collection(ServerTestCollection.KEY)]
    public class TwoFactorAuthTests {
        private readonly ServerTestFixture fx;

        public TwoFactorAuthTests(ServerTestFixture fixture) {
            fx = fixture;
        }

        public async Task<(HttpClient, TotpSetupResponse)> registerAndStartTotpSetup(string username) {
            var client = fx.getClient();
            var authedUser = await AccountRegistrar.registerAccount(client, username);
            client.addUserToken(authedUser);

            var resp = await client.GetAsync("/a/auth/setup2fa");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<TotpSetupResponse>(await resp.Content.ReadAsStringAsync());
            return (client, data);
        }

        public async Task confirmTotpSetup(HttpClient client, string b64Secret) {
            var totpProvider = new TotpProvider(Convert.FromBase64String(b64Secret));

            var resp = await client.PostAsJsonAsync("/a/auth/confirm2fa", new TwoFactorConfirmRequest {
                otpcode = totpProvider.getCode()
            });
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task canGetTwoFactorSetupSecret() {
            var username = AccountRegistrar.TEST_USERNAME + "_setup2fa";
            var (client, totpSetup) = await registerAndStartTotpSetup(username);
            Assert.True(Convert.FromBase64String(totpSetup.secret).Length == TotpProvider.TOTP_SECRET_LENGTH);
        }

        [Fact]
        public async Task canConfirmTwoFactor() {
            var username = AccountRegistrar.TEST_USERNAME + "_confirm2fa";
            var (client, totpSetup) = await registerAndStartTotpSetup(username);
            await confirmTotpSetup(client, totpSetup.secret);
            // check the server context
            Assert.True(fx.serverContext.userManager.findByUsername(username).totpEnabled);
        }

        [Fact]
        public async Task requiresOtpCodeWhenTwoFactorEnabled() {
            var username = AccountRegistrar.TEST_USERNAME + "_req2fa";
            var (client, totpSetup) = await registerAndStartTotpSetup(username);
            await confirmTotpSetup(client, totpSetup.secret);
            // attempt a login, which should fail with FailedDependency (requires OTP)
            var resp = await client.PostAsJsonAsync("/a/auth/login", new LoginUserRequest {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD
            });
            Assert.Equal(HttpStatusCode.FailedDependency, resp.StatusCode);
        }

        [Fact]
        public async Task canLoginTwoFactor() {
            var username = AccountRegistrar.TEST_USERNAME + "_login2fa";
            var (client, totpSetup) = await registerAndStartTotpSetup(username);
            await confirmTotpSetup(client, totpSetup.secret);
            var totpProvider = new TotpProvider(Convert.FromBase64String(totpSetup.secret));
            // attempt a 2fa login
            var resp = await client.PostAsJsonAsync("/a/auth/login2fa", new TwoFactorLoginRequest {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD,
                otpcode = totpProvider.getCode()
            });
            resp.EnsureSuccessStatusCode();
        }
    }
}