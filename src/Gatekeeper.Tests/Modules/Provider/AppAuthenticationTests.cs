using System.Net;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
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
    }
}