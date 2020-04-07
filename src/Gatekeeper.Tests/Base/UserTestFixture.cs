#region

using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Responses;
using Gatekeeper.Tests.Utilities;
using Hexagon.Utilities;

#endregion

namespace Gatekeeper.Tests.Base {
    public class UserTestFixture : ServerTestFixture {
        public string username;
        public AuthedUserResponse authedUser;

        public async Task initialize() {
            // register account
            username = $"{AccountRegistrar.TEST_USERNAME}_{StringUtils.secureRandomString(4)}";
            authedUser = await new AccountRegistrar(serverContext).registerAccount(getClient(), username, true);
        }

        public async Task<HttpClient> getAuthedClient() {
            await initialize();
            var client = base.getClient();
            client.addToken(authedUser.token);
            return client;
        }
    }
}