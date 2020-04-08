using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Gatekeeper.Tests.Utilities;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Provider {
    
    public class RemoteAuthTests {
        private readonly UserTestFixture fx;

        public RemoteAuthTests(UserTestFixture fixture) {
            fx = fixture;
        }

        private async Task<RemoteIdentity> getSaltAppIdentity() {
            var client = await fx.getAuthedClient();

            var resp = await client.GetAsync("/a/app/login/Salt");
            resp.EnsureSuccessStatusCode();
            var appIdentity = JsonConvert.DeserializeObject<RemoteIdentity>(await resp.Content.ReadAsStringAsync());
            Assert.Equal("/Food/Salt", appIdentity.token.scope);
            return appIdentity;
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
            var identity = await getSaltAppIdentity();
            var appClient = getAppClient(identity.token);

            var resp = await appClient.GetAsync("/a/remote/user");
            resp.EnsureSuccessStatusCode(); // valid user info
        }

        [Fact]
        public async Task getRemoteAuthInfo() {
            var identity = await getSaltAppIdentity();
            Assert.Equal(fx.username, identity.user.username);
            var appClient = getAppClient(identity.token);

            var resp = await appClient.GetAsync("/a/remote");
            resp.EnsureSuccessStatusCode();

            // check remote auth info
            var remoteAuth =
                JsonConvert.DeserializeObject<RemoteAuthentication>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(fx.username, remoteAuth.user.username);
        }
    }
}