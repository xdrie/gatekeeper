using System.Collections.Generic;
using System.Linq;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Services.Users {
    public class PermissionResolver : DependencyObject {
        public User user { get; }

        public PermissionResolver(SContext context, User user) : base(context) {
            this.user = user;
        }

        private IEnumerable<Group> groups =>
            user.groups.Select(x => serverContext.config.groups.Single(g => g.name == x));

        public IEnumerable<Permission> aggregatePermissions() {
            return groups.SelectMany(x => x.permissions);
        }
    }
}