#region

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Newtonsoft.Json.Linq;
using Xunit;

#endregion

namespace Gatekeeper.Tests.Auth {
    [Collection(ServerTestCollection.KEY)]
    public class AuthModuleTests {
        private readonly ServerTestFixture fx;
        private readonly HttpClient client;

        public AuthModuleTests(ServerTestFixture fixture) {
            fx = fixture;
            client = fx.getClient();
        }

        private const string TEST_USERNAME = "test";
        public const string TEST_PASSWORD = "1234567890";

        public static Task<HttpResponseMessage> registerAccount(HttpClient client, string username) {
            return client.PostAsJsonAsync("/a/auth/register", new {
                username,
                password = TEST_PASSWORD
            });
        }

        [Theory]
        [InlineData("a")] // too short
        [InlineData(
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // too long
        public async Task registrationRejectsInvalidPasswords(string password) {
            var resp = await client.PostAsJsonAsync("/a/auth/register", new {
                username = TEST_USERNAME,
                password
            });
            Assert.Equal(HttpStatusCode.UnprocessableEntity, resp.StatusCode);
        }

        [Fact]
        public async Task canRegisterAccount() {
            var username = TEST_USERNAME + "_reg";
            var resp = await registerAccount(client, username);
            resp.EnsureSuccessStatusCode();
            var data = JObject.Parse(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data["username"]);
            Assert.NotNull(data["token"]);
            Assert.NotNull(data["userid"]);
        }

        [Fact]
        public async Task canLoginAccount() {
            var username = TEST_USERNAME + "_login";
            var regResponse = await registerAccount(client, username);
            regResponse.EnsureSuccessStatusCode();
            // now attempt to log in
            var resp = await client.PostAsJsonAsync("/a/auth/login", new {
                username,
                password = TEST_PASSWORD
            });
            var data = JObject.Parse(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data["username"]);
            Assert.NotNull(data["token"]);
            Assert.NotNull(data["userid"]);
        }

        [Fact]
        public async Task canReauthAccount() {
            var username = TEST_USERNAME + "_reauth";
            var regResponse = await registerAccount(client, username);
            regResponse.EnsureSuccessStatusCode();
            var regData = JObject.Parse(await regResponse.Content.ReadAsStringAsync());
            var token = regData["token"];
            // attempt to reauth using token
            var resp = await client.PostAsJsonAsync("/a/auth/reauth", new {
                username,
                token
            });
            var data = JObject.Parse(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data["username"]);
            Assert.Equal(token, data["token"]);
            Assert.NotNull(data["userid"]);
        }

        [Fact]
        public async Task canDeleteAccount() {
            var username = TEST_USERNAME + "_delete";
            var regResponse = await registerAccount(client, username);
            regResponse.EnsureSuccessStatusCode();
            var regData = JObject.Parse(await regResponse.Content.ReadAsStringAsync());
            var resp = await client.PostAsJsonAsync("/a/auth/delete", new {
                username,
                password = TEST_PASSWORD
            });
            resp.EnsureSuccessStatusCode();
        }
    }
}