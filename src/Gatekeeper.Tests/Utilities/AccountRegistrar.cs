using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Models.Validators;
using Hexagon.Models;
using Newtonsoft.Json;

namespace Gatekeeper.Tests.Utilities {
    public class AccountRegistrar : DependencyService<SContext> {
        public const string TEST_USERNAME = "test";
        public const string TEST_PASSWORD = "1234567890";

        public AccountRegistrar(SContext context) : base(context) { }

        public async Task<AuthedUserResponse> registerAccount(HttpClient client, string username) {
            var resp = await client.PostAsJsonAsync("/a/auth/create", new RegisterRequest {
                username = username,
                name = "Test User",
                email = $"{username}@example.local",
                password = TEST_PASSWORD,
                pronouns = User.Pronouns.TheyThem.ToString(),
                isRobot = AuthRequestValidators.RegisterRequestValidator.NOT_ROBOT_PROMISE
            });
            resp.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<AuthedUserResponse>(await resp.Content.ReadAsStringAsync());
        }

        // registerAccount(HttpClient client, string username, bool admin = false) {
        //     // register an account using the bridge connection
        //     var resp = await client.PostAsJsonAsync("/a/bridge",
        //         AccountRegistrarHelpers.createMockGateToken(username, admin));
        //     resp.EnsureSuccessStatusCode();
        //     var user = JsonConvert.DeserializeObject<GateUser>(await resp.Content.ReadAsStringAsync());
        //     return user;
        // }
    }

    public static class AccountRegistrarHelpers {
        public static void addBearerToken(this HttpClient client, Token token) {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.content);
        }
    }
}