using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
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

        private async Task<Token> getSaltAppToken() {
            var client = await fx.getAuthedClient();

            var resp = await client.GetAsync("/a/app/token/Salt");
            resp.EnsureSuccessStatusCode();
            var appToken = JsonConvert.DeserializeObject<Token>(await resp.Content.ReadAsStringAsync());
            Assert.Equal("/Food/Salt", appToken.scope);
            return appToken;
        }

        private HttpClient getAppClient(Token appToken) {
            // authenticate a client on behalf of the application
            var appClient = fx.getClient();
            appClient.addToken(appToken);
            appClient.DefaultRequestHeaders.Add(Gatekeeper.Constants.APP_SECRET_HEADER, Constants.Apps.APP_SECRET);
            return appClient;
        }

        [Fact]
        public async Task getRemoteUserInfo() {
            var appClient = getAppClient(await getSaltAppToken());

            var resp = await appClient.GetAsync("/a/remote/user");
            resp.EnsureSuccessStatusCode(); // valid user info
        }

        [Fact]
        public async Task getRemoteAuthInfo() {
            var appClient = getAppClient(await getSaltAppToken());

            var resp = await appClient.GetAsync("/a/remote");
            resp.EnsureSuccessStatusCode();
        }
    }
}