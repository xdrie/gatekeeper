#region

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Gatekeeper.Tests.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

#endregion

namespace Gatekeeper.Tests.Modules.Auth {
    [Collection(ServerTestCollection.KEY)]
    public class AuthModuleTests {
        private readonly ServerTestFixture fx;

        public AuthModuleTests(ServerTestFixture fixture) {
            fx = fixture;
        }

        [Fact]
        public async Task canRegisterAccount() {
            var client = fx.getClient();
            var username = AccountRegistrar.TEST_USERNAME + "_reg";
            var resp = await AccountRegistrar.registerAccount(client, username);
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<AuthedUserResponse>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data.user.username);
            Assert.NotNull(data.token.content);
            Assert.Equal(data.token.scope, AccessScope.ROOT_PATH);
        }

        [Fact]
        public async Task canLoginAccount() {
            var client = fx.getClient();
            var username = AccountRegistrar.TEST_USERNAME + "_login";
            var regResponse = await AccountRegistrar.registerAccount(client, username);
            regResponse.EnsureSuccessStatusCode();
            // now attempt to log in
            var resp = await client.PostAsJsonAsync("/a/auth/login", new UserLoginRequest {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD
            });
            var data = JsonConvert.DeserializeObject<AuthedUserResponse>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data.user.username);
            Assert.NotNull(data.token.content);
            Assert.True(data.token.expires > DateTime.Now);
        }

        [Fact]
        public async Task canDeleteAccount() {
            var client = fx.getClient();
            var username = AccountRegistrar.TEST_USERNAME + "_delete";
            var regResponse = await AccountRegistrar.registerAccount(client, username);
            regResponse.EnsureSuccessStatusCode();
            var regData = JObject.Parse(await regResponse.Content.ReadAsStringAsync());
            var resp = await client.PostAsJsonAsync("/a/auth/delete", new UserLoginRequest {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD
            });
            resp.EnsureSuccessStatusCode();
        }
    }
}