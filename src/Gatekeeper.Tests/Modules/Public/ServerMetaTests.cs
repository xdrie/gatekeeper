using System.Threading.Tasks;
using Gatekeeper.Models.Meta;
using Gatekeeper.Server.Config;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Newtonsoft.Json;
using Xunit;

namespace Gatekeeper.Tests.Modules.Public {
    public class ServerMetaTests : ServerDependentTests {
        public ServerMetaTests(ServerTestFixture fixture) : base(fixture) { }

        [Fact]
        public async Task canGetServerMetadata() {
            // check meta page
            var resp = await client.GetAsync($"/a/meta");
            resp.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<ServerMetadata>(await resp.Content.ReadAsStringAsync());
            Assert.Equal(SConfig.VERSION, data.version);
        }
    }
}