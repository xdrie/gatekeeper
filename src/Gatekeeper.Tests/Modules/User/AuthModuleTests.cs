#region

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

#endregion

namespace Gatekeeper.Tests.Modules.User {
    [Collection(ServerTestCollection.KEY)]
    public class AuthModuleTests {
        private readonly ServerTestFixture fx;
        private readonly HttpClient client;

        public AuthModuleTests(ServerTestFixture fixture) {
            fx = fixture;
            client = fx.getClient();
        }

        public const string TEST_USERNAME = "test";
        public const string TEST_NAME = "Test Testingtest";
        public const string TEST_EMAIL = "test@example.com";
        public const string TEST_PASSWORD = "1234567890";

        public static Task<HttpResponseMessage> registerAccount(HttpClient client, string username) {
            return client.PostAsJsonAsync("/a/user/create", new UserCreateRequest {
                username = username,
                name = TEST_NAME,
                email = TEST_EMAIL,
                password = TEST_PASSWORD,
                pronouns = Models.Identity.User.Pronouns.TheyThem.ToString(),
                isRobot = UserCreateRequest.Validator.NOT_ROBOT_PROMISE
            });
        }

        [Fact]
        public async Task canRegisterAccount() {
            var username = TEST_USERNAME + "_reg";
            var resp = await registerAccount(client, username);
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<AuthedUserResponse>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data.user.username);
            Assert.NotNull(data.token.content);
        }

        [Fact]
        public async Task canLoginAccount() {
            var username = TEST_USERNAME + "_login";
            var regResponse = await registerAccount(client, username);
            regResponse.EnsureSuccessStatusCode();
            // now attempt to log in
            var resp = await client.PostAsJsonAsync("/a/user/login", new UserLoginRequest {
                username = username,
                password = TEST_PASSWORD
            });
            var data = JsonConvert.DeserializeObject<AuthedUserResponse>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data.user.username);
            Assert.NotNull(data.token.content);
            Assert.True(data.token.expires > DateTime.Now);
        }

        [Fact]
        public async Task canDeleteAccount() {
            var username = TEST_USERNAME + "_delete";
            var regResponse = await registerAccount(client, username);
            regResponse.EnsureSuccessStatusCode();
            var regData = JObject.Parse(await regResponse.Content.ReadAsStringAsync());
            var resp = await client.PostAsJsonAsync("/a/user/delete", new UserLoginRequest {
                username = username,
                password = TEST_PASSWORD
            });
            resp.EnsureSuccessStatusCode();
        }
    }
}