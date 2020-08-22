using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Tests.Utilities;
using Iri.Glass.Utilities;
using Xunit;

namespace Gatekeeper.Tests.Base {
    public abstract class UserDependentTests : ServerDependentTests, IAsyncLifetime {
        protected HttpClient client;
        protected AuthenticatedUser user;
        protected Token token;
        protected string username => user.username;
        protected string userId => user.uuid;

        public UserDependentTests(ServerTestFixture fx) : base(fx) { }

        public virtual async Task InitializeAsync() {
            client = fx.getClient();
            var registrar = new AccountRegistrar(fx.serverContext);
            var username = $"{AccountRegistrar.TEST_USERNAME}_{StringUtils.secureRandomString(8)}";
            var authedUser = await registrar.registerAccount(client, username);
            user = authedUser.user;
            token = authedUser.token;
            client.addBearerToken(token); // store auth in client
            // verify user
            var findUser = fx.serverContext.userManager.findByUsername(username);
            findUser.role = User.Role.User;
            fx.serverContext.userManager.updateUser(findUser);
        }

        public virtual Task DisposeAsync() {
            return Task.CompletedTask;
        }
    }
}