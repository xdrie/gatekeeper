using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Remote;
using Gatekeeper.Services.Auth;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Gatekeeper.Tests.Utilities;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Provider {
    [Collection(UserTestCollection.KEY)]
    public class RemoteAuthTests {
        private readonly UserTestFixture fx;

        public RemoteAuthTests(UserTestFixture fixture) {
            fx = fixture;
        }

        [Fact]
        public async Task appTokensGetUserInfo() {
            await fx.initialize();
            var client = fx.getAuthedClient();

            var resp = await client.GetAsync("/a/app/token/Salt");
            resp.EnsureSuccessStatusCode();
            var appToken = JsonConvert.DeserializeObject<Token>(await resp.Content.ReadAsStringAsync());
            Assert.Equal("/Food/Salt", appToken.scope);

            // now try requesting user info, but as the "application"
            var appClient = fx.getClient(); // set up a client as the application
            appClient.addToken(appToken);
            appClient.DefaultRequestHeaders.Add(ApiAuthenticator.APP_SECRET_HEADER, Constants.Apps.APP_SECRET);
            // request user info
            var userInfoResp = await appClient.GetAsync("/a/remote/user");
            userInfoResp.EnsureSuccessStatusCode(); // valid user info
        }
    }
}