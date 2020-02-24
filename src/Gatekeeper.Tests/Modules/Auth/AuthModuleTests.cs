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
            var authedUser = await AccountRegistrar.registerAccount(client, username);
            Assert.Equal(username, authedUser.user.username);
            Assert.NotNull(authedUser.token.content);
            Assert.Equal(authedUser.token.scope, AccessScope.ROOT_PATH);
        }

        [Fact]
        public async Task canLoginAccount() {
            var client = fx.getClient();
            var username = AccountRegistrar.TEST_USERNAME + "_login";
            await AccountRegistrar.registerAccount(client, username);
            // now attempt to log in
            var resp = await client.PostAsJsonAsync("/a/auth/login", new LoginRequest {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD
            });
            var data = JsonConvert.DeserializeObject<AuthedUserResponse>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data.user.username);
            Assert.NotNull(data.token.content);
            Assert.True(data.token.expires > DateTime.Now);
        }

        [Fact]
        public async Task canVerifyAccount() {
            var client = fx.getClient();
            var username = AccountRegistrar.TEST_USERNAME + "_verify";
            var authedUser = await AccountRegistrar.registerAccount(client, username);
            client.addToken(authedUser.token);
            // fetch the verification code manually
            var verificationCode = fx.serverContext.userManager.findByUsername(username).verification;
            // now attempt to log in
            var resp = await client.PostAsync($"/a/auth/verify/{verificationCode}", null);
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task canDeleteAccount() {
            var client = fx.getClient();
            var username = AccountRegistrar.TEST_USERNAME + "_delete";
            await AccountRegistrar.registerAccount(client, username);
            var resp = await client.PostAsJsonAsync("/a/auth/delete", new LoginRequest {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD
            });
            resp.EnsureSuccessStatusCode();
        }
    }
}