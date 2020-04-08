using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Users {
    public class SelfModuleTests : UserDependentTests {
        public SelfModuleTests(ServerTestFixture fx) : base(fx) { }

        [Fact]
        public async Task canAccessMePage() {
            // check me page
            var resp = await client.GetAsync("/a/u/me");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<AuthenticatedUser>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data.username);
        }

        [Fact]
        public async Task canGetGroupMembership() {
            var resp = await client.GetAsync("/a/u/groups");
            resp.EnsureSuccessStatusCode();
            var groupList = JsonConvert.DeserializeObject<string[]>(await resp.Content.ReadAsStringAsync());
            var userModel = fx.serverContext.userManager.findByUsername(username);
            Assert.Equal(userModel.groups, groupList);
        }

        [Fact]
        public async Task canGetAppAccessRules() {
            var resp = await client.GetAsync("/a/u/rules");
            resp.EnsureSuccessStatusCode();
        }
    }
}