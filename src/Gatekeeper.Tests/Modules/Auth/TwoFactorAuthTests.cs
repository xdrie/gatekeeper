using System;
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

        [Fact]
        public async Task canGetTwoFactorSetupSecret() {
            var client = fx.getClient();
            var username = AccountRegistrar.TEST_USERNAME + "_reg";
            var regResp = await AccountRegistrar.registerAccount(client, username);
            regResp.EnsureSuccessStatusCode();

            var resp = await client.GetAsync("/a/auth/setup2fa");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<TotpSetupResponse>(await resp.Content.ReadAsStringAsync());
            Assert.True(Convert.FromBase64String(data.secret).Length == TotpProvider.TOTP_SECRET_LENGTH);
        }

        [Fact]
        public async Task canConfirmTwoFactor() {
            var client = fx.getClient();
            var username = AccountRegistrar.TEST_USERNAME + "_reg";
            var regResp = await AccountRegistrar.registerAccount(client, username);
            regResp.EnsureSuccessStatusCode();
            
            var twoFactorSetupResp = await client.GetAsync("/a/auth/setup2fa");
            twoFactorSetupResp.EnsureSuccessStatusCode();
            var setupResponse = JsonConvert.DeserializeObject<TotpSetupResponse>(await twoFactorSetupResp.Content.ReadAsStringAsync());
            
            var totpProvider = new TotpProvider(Convert.FromBase64String(setupResponse.secret));

            var resp = await client.PostAsJsonAsync("/a/auth/confirm2fa", new TwoFactorConfirmRequest {
                otpcode = totpProvider.getCode()
            });
            resp.EnsureSuccessStatusCode();
        }
    }
}