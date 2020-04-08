using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Utilities;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Provider {
    public class RemoteAuthTests : UserDependentTests {
        private RemoteIdentity identity;
        private HttpClient appClient;
        public RemoteAuthTests(ServerTestFixture fx) : base(fx) { }

        public override async Task InitializeAsync() {
            await base.InitializeAsync();

            var resp = await client.GetAsync("/a/app/login/Salt");
            resp.EnsureSuccessStatusCode();
            identity = JsonConvert.DeserializeObject<RemoteIdentity>(await resp.Content.ReadAsStringAsync());
            Assert.Equal("/Food/Salt", identity.token.scope);
            Assert.Equal(username, identity.user.username);

            // authenticate a client on behalf of the application
            appClient = fx.getClient();
            appClient.addBearerToken(identity.token);
            appClient.DefaultRequestHeaders.Add(Gatekeeper.Constants.APP_SECRET_HEADER, Constants.Apps.APP_SECRET);
        }

        [Fact]
        public async Task getRemoteUserInfo() {
            var resp = await appClient.GetAsync("/a/remote/user");
            resp.EnsureSuccessStatusCode(); // valid user info
        }

        [Fact]
        public async Task getRemoteAuthInfo() {
            var resp = await appClient.GetAsync("/a/remote");
            resp.EnsureSuccessStatusCode();

            // check remote auth info
            var remoteAuth =
                JsonConvert.DeserializeObject<RemoteAuthentication>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, remoteAuth.user.username);
        }
    }
}