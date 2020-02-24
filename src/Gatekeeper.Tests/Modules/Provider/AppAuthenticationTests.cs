using System.Net;
using System.Threading.Tasks;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Identity;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Gatekeeper.Tests.Utilities;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Provider {
    [Collection(UserTestCollection.KEY)]
    public class AppAuthenticationTests {
        private readonly UserTestFixture fx;

        public AppAuthenticationTests(UserTestFixture fixture) {
            fx = fixture;
        }

        [Fact]
        public async Task rejectsUnauthorizedApp() {
            await fx.initialize();
            var client = fx.getAuthedClient();

            var resp = await client.GetAsync("/a/app/token/BeanCan");
            Assert.Equal(HttpStatusCode.Forbidden, resp.StatusCode);
        }
        
        [Fact]
        public async Task authorizesGlobalApp() {
            await fx.initialize();
            var client = fx.getAuthedClient();

            var resp = await client.GetAsync("/a/app/token/Global");
            resp.EnsureSuccessStatusCode();
            var appToken = JsonConvert.DeserializeObject<Token>(await resp.Content.ReadAsStringAsync());
            Assert.Equal("*/Global", appToken.scope);
        }

        [Fact]
        public async Task authorizesAllowedApp() {
            await fx.initialize();
            var client = fx.getAuthedClient();
            // add permission to access salt shaker
            var user = fx.serverContext.userManager.findByUsername(fx.authedUser.user.username);
            var cheapFoodPerm = new Permission("/CheapFood");
            user.permissions.Add(cheapFoodPerm);
            fx.serverContext.userManager.updateUser(user);

            var resp = await client.GetAsync("/a/app/token/SaltShaker");
            resp.EnsureSuccessStatusCode();
            var appToken = JsonConvert.DeserializeObject<Token>(await resp.Content.ReadAsStringAsync());
            Assert.Equal("/CheapFood/SaltShaker", appToken.scope);

            // clean up
            user.permissions.Remove(cheapFoodPerm);
            fx.serverContext.userManager.updateUser(user);
        }
        
        [Fact]
        public async Task appTokensBlockedFromRootScope() {
            await fx.initialize();
            var client = fx.getAuthedClient();

            var resp = await client.GetAsync("/a/app/token/Global");
            resp.EnsureSuccessStatusCode();
            var appToken = JsonConvert.DeserializeObject<Token>(await resp.Content.ReadAsStringAsync());
            
            // now try requesting user info, but as the "application"
            var appClient = fx.getClient(); // set up a client as the application
            appClient.addToken(appToken);
            // check me page
            var mePageResp = await client.GetAsync("/a/u/me");
            Assert.Equal(HttpStatusCode.Unauthorized, mePageResp.StatusCode); // we should be barred, because this is a scoped token
        }
    }
}