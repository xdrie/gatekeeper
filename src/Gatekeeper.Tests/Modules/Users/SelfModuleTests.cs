using System.Net.Http;
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
            await fx.initialize();
            var client = fx.getAuthedClient();
            
            // check me page
            var resp = await client.GetAsync("/a/u/me");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<AuthenticatedUser>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(fx.username, data.username);
        }
    }
}