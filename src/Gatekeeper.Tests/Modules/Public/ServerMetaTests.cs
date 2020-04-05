using System.Threading.Tasks;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Meta;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Public {
    [Collection(ServerTestCollection.KEY)]
    public class ServerMetaTests {
        private readonly ServerTestFixture fx;

        public ServerMetaTests(ServerTestFixture fixture) {
            fx = fixture;
        }

        [Fact]
        public async Task canGetServerMetadata() {
            var client = fx.getClient();
            
            // check meta page
            var resp = await client.GetAsync($"/a/meta");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<ServerMetadata>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(SConfig.VERSION, data.version);
        }
    }
}