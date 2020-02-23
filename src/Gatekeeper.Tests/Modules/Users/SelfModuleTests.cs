using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Xunit;

namespace Gatekeeper.Tests.Modules.Users {
    [Collection(UserTestCollection.KEY)]
    public class SelfModuleTests {
        private readonly UserTestFixture fx;
        private readonly HttpClient client;

        public SelfModuleTests(UserTestFixture fixture) {
            fx = fixture;
            client = fx.getClient();
        }

        [Fact]
        public async Task canAccessMePage() {
            await fx.initialize();
            
            // TODO: check me page
        }
    }
}