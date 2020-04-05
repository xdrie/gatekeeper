using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Server.Models.Identity;
using Gatekeeper.Server.Models.Requests;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Meta;
using Gatekeeper.Tests.Utilities;
using Xunit;

namespace Gatekeeper.Tests.Modules.Manager {
    [Collection(UserTestCollection.KEY)]
    public class GroupManagementTests {
        private readonly UserTestFixture fx;

        public GroupManagementTests(UserTestFixture fixture) {
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
        public async Task canAddAndRemoveGroups() {
            await fx.initialize();

            var username = AccountRegistrar.TEST_ADMIN + "_mgperms";
            var adminClient = await registerAdminAccount(username);

            // get the friends group
            var luxGroup = fx.serverContext.config.groups.Single(x => x.name == "Luxurious");
            // attempt to add a permission
            var addResp = await adminClient.PatchAsJsonAsync("/a/groups/update", new UpdateGroupRequest {
                userUuid = fx.authedUser.user.uuid,
                type = "add",
                groups = new[] {luxGroup.name}
            });
            addResp.EnsureSuccessStatusCode();
            // ensure that it was added
            var addedToUser = fx.serverContext.userManager.findByUsername(fx.username);
            addedToUser = fx.serverContext.userManager.loadGroups(addedToUser);
            Assert.Contains(addedToUser.groups, x => x == luxGroup.name);
            // remove the permission
            var removeResp = await adminClient.PatchAsJsonAsync("/a/groups/update", new UpdateGroupRequest {
                userUuid = fx.authedUser.user.uuid,
                type = "remove",
                groups = new[] {luxGroup.name}
            });
            removeResp.EnsureSuccessStatusCode();
            // ensure that it was removed
            var removedFromUser = fx.serverContext.userManager.findByUsername(fx.username);
            removedFromUser = fx.serverContext.userManager.loadGroups(removedFromUser);
            Assert.DoesNotContain(removedFromUser.groups, x => x == luxGroup.name);
        }
    }
}