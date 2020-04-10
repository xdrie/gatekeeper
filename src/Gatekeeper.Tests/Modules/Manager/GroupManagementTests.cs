using System.Linq;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Tests.Base;
using Gatekeeper.Tests.Utilities;
using Xunit;

namespace Gatekeeper.Tests.Modules.Manager {
    public class GroupManagementTests : UserDependentTests {
        public GroupManagementTests(ServerTestFixture fx) : base(fx) { }

        public override async Task InitializeAsync() {
            await base.InitializeAsync();
            // promote user to admin
            var findUser = fx.serverContext.userManager.findByUsername(username);
            findUser.role = User.Role.Admin;
            fx.serverContext.userManager.updateUser(findUser);
        }

        [Fact]
        public async Task canAddAndRemoveGroups() {
            // get the friends group
            var luxGroup = fx.serverContext.config.groups.Single(x => x.name == "Luxurious");
            // attempt to add a permission
            var addResp = await client.PatchAsJsonAsync("/a/groups/update", new UpdateGroupRequest {
                userUuid = user.uuid,
                type = "add",
                groups = new[] {luxGroup.name}
            });
            addResp.EnsureSuccessStatusCode();
            // ensure that it was added
            var addedToUser = fx.serverContext.userManager.findByUsername(username);
            addedToUser = fx.serverContext.userManager.loadGroups(addedToUser);
            Assert.Contains(addedToUser.groups, x => x == luxGroup.name);
            // remove the permission
            var removeResp = await client.PatchAsJsonAsync("/a/groups/update", new UpdateGroupRequest {
                userUuid = user.uuid,
                type = "remove",
                groups = new[] {luxGroup.name}
            });
            removeResp.EnsureSuccessStatusCode();
            // ensure that it was removed
            var removedFromUser = fx.serverContext.userManager.findByUsername(username);
            removedFromUser = fx.serverContext.userManager.loadGroups(removedFromUser);
            Assert.DoesNotContain(removedFromUser.groups, x => x == luxGroup.name);
        }
    }
}