using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Tests.Base;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Users {
    public class UserDirectoryTests : UserDependentTests {
        public UserDirectoryTests(ServerTestFixture fx) : base(fx) { }

        [Fact]
        public async Task canFetchMeFromDirectory() {
            // check me page
            var resp = await client.GetAsync($"/a/u/{username}");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<PublicUser>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(username, data.username);
        }
    }
}