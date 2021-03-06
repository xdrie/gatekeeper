using System.Net;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Utilities;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Provider {
    public class AppAuthenticationTests : UserDependentTests {
        public AppAuthenticationTests(ServerTestFixture fx) : base(fx) { }

        [Fact]
        public async Task rejectsUnauthorizedApp() {
            var resp = await client.GetAsync("/a/app/token/Hotels");
            Assert.Equal(HttpStatusCode.Forbidden, resp.StatusCode);
        }

        [Fact]
        public async Task authorizesAllowedApp() {
            var resp = await client.GetAsync("/a/app/token/Salt");
            resp.EnsureSuccessStatusCode();
            var appToken = JsonConvert.DeserializeObject<Token>(await resp.Content.ReadAsStringAsync());
            Assert.Equal("/Food/Salt", appToken.scope);
        }

        [Fact]
        public async Task appTokensBlockedFromRootScope() {
            var resp = await client.GetAsync("/a/app/token/Salt");
            resp.EnsureSuccessStatusCode();
            var appToken = JsonConvert.DeserializeObject<Token>(await resp.Content.ReadAsStringAsync());
            Assert.Equal("/Food/Salt", appToken.scope);

            // now try requesting user info, but as the "application"
            var appClient = fx.getClient(); // set up a client as the application
            appClient.addBearerToken(appToken);
            // check me page
            var mePageResp = await appClient.GetAsync("/a/u/me");
            Assert.Equal(HttpStatusCode.Unauthorized,
                mePageResp.StatusCode); // we should be barred, because this is a scoped token
        }
    }
}