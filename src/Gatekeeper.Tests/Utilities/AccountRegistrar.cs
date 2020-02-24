using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Newtonsoft.Json;

namespace Gatekeeper.Tests.Utilities {
    public static class AccountRegistrar {
        public const string TEST_USERNAME = "test";
        public const string TEST_NAME = "Test Testingtest";
        public const string TEST_EMAIL = "test@example.com";
        public const string TEST_PASSWORD = "1234567890";

        public static async Task<AuthedUserResponse> registerAccount(HttpClient client, string username) {
            var resp = await client.PostAsJsonAsync("/a/auth/create", new UserCreateRequest {
                username = username,
                name = TEST_NAME,
                email = TEST_EMAIL,
                password = TEST_PASSWORD,
                pronouns = Models.Identity.User.Pronouns.TheyThem.ToString(),
                isRobot = UserCreateRequest.Validator.NOT_ROBOT_PROMISE
            });
            resp.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<AuthedUserResponse>(await resp.Content.ReadAsStringAsync());
        }

        public static void addUserToken(this HttpClient client, AuthedUserResponse authedUser) {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authedUser.token.content);
        }
    }
}