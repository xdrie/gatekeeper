using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Gatekeeper.Tests.Utilities;
using Xunit;

namespace Gatekeeper.Tests.Modules.Manager {
    [Collection(UserTestCollection.KEY)]
    public class PermissionManagementTests {
        private readonly UserTestFixture fx;

        public PermissionManagementTests(UserTestFixture fixture) {
            fx = fixture;
        }

        public async Task<HttpClient> registerAdminAccount(string username) {
            var client = fx.getClient();
            var authedUser =
                await new AccountRegistrar(fx.serverContext).registerAccount(client, username, verify: true);
            client.addToken(authedUser.token);

            // promote user to admin
            var user = fx.serverContext.userManager.findByUsername(authedUser.user.username);
            user.role = User.Role.Admin;
            fx.serverContext.userManager.updateUser(user);

            return client;
        }

        [Fact]
        public async Task canAddAndRemovePermissions() {
            await fx.initialize();
            
            var username = AccountRegistrar.TEST_ADMIN + "_mgperms";
            var adminClient = await registerAdminAccount(username);

            var testPerm = "/AdminTestScope";
            // attempt to add a permission
            var addResp = await adminClient.PatchAsJsonAsync("/a/perms/update", new UpdateGroupRequest {
                userUuid = fx.authedUser.user.uuid,
                type = "add",
                groups = new[] {testPerm}
            });
            addResp.EnsureSuccessStatusCode();
            // ensure that it was added
            var addedToUser = fx.serverContext.userManager.findByUsername(fx.username);
            addedToUser = fx.serverContext.userManager.loadGroups(addedToUser);
            Assert.Contains(addedToUser.permissions, x => x.path == testPerm);
            // remove the permission
            var removeResp = await adminClient.PatchAsJsonAsync("/a/perms/update", new UpdateGroupRequest {
                userUuid = fx.authedUser.user.uuid,
                type = "remove",
                groups = new[] {testPerm}
            });
            removeResp.EnsureSuccessStatusCode();
            // ensure that it was removed
            var removedFromUser = fx.serverContext.userManager.findByUsername(fx.username);
            removedFromUser = fx.serverContext.userManager.loadGroups(removedFromUser);
            Assert.DoesNotContain(removedFromUser.permissions, x => x.path == testPerm);
        }
    }
}