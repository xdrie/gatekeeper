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
using Xunit;

#endregion

namespace Gatekeeper.Tests.Modules.Auth {
    public class AuthModuleTests : UserDependentTests {
        public AuthModuleTests(ServerTestFixture fx) : base(fx) { }

        [Fact]
        public async Task canRegisterAccount() {
            Assert.Equal(username, user.username);
            Assert.NotNull(token.content);
            Assert.Equal(token.scope, AccessScope.ROOT_PATH);
        }

        [Fact]
        public async Task canLoginAccount() {
            // now attempt to log in
            var resp = await client.PostAsJsonAsync("/a/auth/login", new LoginRequest {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD
            });
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<AuthedUserResponse>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data.user.username);
            Assert.NotNull(data.token.content);
            Assert.False(data.token.expired());
        }

        [Fact]
        public async Task canDeleteAccount() {
            var resp = await client.PostAsJsonAsync("/a/auth/delete", new LoginRequest {
                username = username,
                password = AccountRegistrar.TEST_PASSWORD
            });
            resp.EnsureSuccessStatusCode();
            // now confirm that the user has been deleted
            Assert.Null(fx.serverContext.userManager.findByUsername(username));
        }
    }
}