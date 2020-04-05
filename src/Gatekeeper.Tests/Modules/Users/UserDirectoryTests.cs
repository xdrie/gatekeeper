using System.Threading.Tasks;
using Gatekeeper.Server.Models.Identity;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Users {
    [Collection(UserTestCollection.KEY)]
    public class UserDirectoryTests {
        private readonly UserTestFixture fx;

        public UserDirectoryTests(UserTestFixture fixture) {
            fx = fixture;
        }

        [Fact]
        public async Task canFetchMeFromDirectory() {
            var client = await fx.getAuthedClient();

            // check me page
            var resp = await client.GetAsync($"/a/u/{fx.username}");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<PublicUser>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(fx.username, data.username);
        }
    }
}