using System.Net.Http;
using Gatekeeper.Tests.Meta;
using Xunit;

namespace Gatekeeper.Tests.Base {
    [Collection(ServerTestCollection.KEY)]
    public abstract class ServerDependentTests {
        protected readonly ServerTestFixture fx;
        protected readonly HttpClient client;

        public ServerDependentTests(ServerTestFixture fixture) {
            fx = fixture;
            client = fx.getClient();
        }
    }
}