using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Models;
using Gatekeeper.Server.Models.Identity;
using Gatekeeper.Server.Models.Requests;
using Gatekeeper.Server.Models.Responses;
using Newtonsoft.Json;

namespace Gatekeeper.Tests.Utilities {
    public class AccountRegistrar : DependencyObject {
        public const string TEST_USERNAME = "test";
        public const string TEST_NAME = "Test Testingtest";
        public const string TEST_EMAIL = "test@example.com";
        public const string TEST_PASSWORD = "1234567890";
        
        public const string TEST_ADMIN = "admin";

        public AccountRegistrar(SContext context) : base(context) { }

        public async Task<AuthedUserResponse> registerAccount(HttpClient client, string username,
            bool verify = false) {
            var resp = await client.PostAsJsonAsync("/a/auth/create", new RegisterRequest {
                username = username,
                name = TEST_NAME,
                email = TEST_EMAIL,
                password = TEST_PASSWORD,
                pronouns = User.Pronouns.TheyThem.ToString(),
                isRobot = RegisterRequest.Validator.NOT_ROBOT_PROMISE
            });
            resp.EnsureSuccessStatusCode();
            if (verify) {
                var userToVerify = serverContext.userManager.findByUsername(username);
                userToVerify.role = User.Role.User;
                serverContext.userManager.updateUser(userToVerify);
            }
            return JsonConvert.DeserializeObject<AuthedUserResponse>(await resp.Content.ReadAsStringAsync());
        }
    }

    public static class AccountRegistrarHelpers {
        public static void addToken(this HttpClient client, Token token) {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.content);
        }
    }
}