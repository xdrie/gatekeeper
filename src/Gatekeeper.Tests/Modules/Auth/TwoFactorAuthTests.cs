using System;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Responses;
using Gatekeeper.Services.Auth;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Auth {
    [Collection(UserTestCollection.KEY)]
    public class TwoFactorAuthTests {
        private readonly UserTestFixture fx;

        public TwoFactorAuthTests(UserTestFixture fixture) {
            fx = fixture;
        }

        [Fact]
        public async Task canGetTwoFactorSetupSecret() {
            await fx.initialize();
            var client = fx.getAuthedClient();

            var resp = await client.GetAsync("/a/auth/setup2fa");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<TotpSetupResponse>(await resp.Content.ReadAsStringAsync());
            Assert.True(Convert.FromBase64String(data.secret).Length == TotpProvider.TOTP_SECRET_LENGTH);
        }
    }
}