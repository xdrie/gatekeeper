using System.Threading.Tasks;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Utilities;
using Xunit;

namespace Gatekeeper.Tests.Modules.Auth {
    public class VerifyModuleTests : ServerDependentTests {
        private AccountRegistrar registrar;

        public VerifyModuleTests(ServerTestFixture fixture) : base(fixture) {
            registrar = new AccountRegistrar(fx.serverContext);
        }

        [Fact]
        public async Task canVerifyAccount() {
            var authedUser = await registrar.registerAccount(client, $"{AccountRegistrar.TEST_USERNAME}_verify");
            var user = authedUser.user;
            client.addBearerToken(authedUser.token);
            // fetch the verification code manually
            var verificationCode = fx.serverContext.userManager.findByUsername(user.username).verification;
            // call verification endpoint
            var resp = await client.PostAsync($"/a/auth/verify/{user.uuid}/{verificationCode}", null);
            resp.EnsureSuccessStatusCode();
        }
    }
}