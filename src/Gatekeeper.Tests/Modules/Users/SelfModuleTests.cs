using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Users {
    [Collection(UserTestCollection.KEY)]
    public class SelfModuleTests {
        private readonly UserTestFixture fx;

        public SelfModuleTests(UserTestFixture fixture) {
            fx = fixture;
        }

        [Fact]
        public async Task canAccessMePage() {
            var client = await fx.getAuthedClient();

            // check me page
            var resp = await client.GetAsync("/a/u/me");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<AuthenticatedUser>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(fx.username, data.username);
        }

        [Fact]
        public async Task canGetGroupMembership() {
            var client = await fx.getAuthedClient();

            var resp = await client.GetAsync("/a/u/groups");
            resp.EnsureSuccessStatusCode();
            var groupList = JsonConvert.DeserializeObject<string[]>(await resp.Content.ReadAsStringAsync());
            var userModel = fx.serverContext.userManager.findByUsername(fx.username);
            Assert.Equal(userModel.groups, groupList);
        }

        [Fact]
        public async Task canGetAppAccessRules() {
            var client = await fx.getAuthedClient();

            var resp = await client.GetAsync("/a/u/rules");
            resp.EnsureSuccessStatusCode();
        }
    }
}