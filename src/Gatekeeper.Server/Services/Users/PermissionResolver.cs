using System.Collections.Generic;
using System.Linq;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Identity;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Models;

namespace Gatekeeper.Server.Services.Users {
    public class PermissionResolver : DependencyObject {
        public User user { get; }

        public PermissionResolver(SContext context, User user) : base(context) {
            this.user = user;
        }

        private IEnumerable<Group> groups =>
            user.groups.Select(x => serverContext.config.groups.Single(g => g.name == x));

        public IEnumerable<Permission> aggregatePermissions() =>
            new GroupPermissionResolver(groups).aggregatePermissions();

        public List<AccessRule> aggregateRules() =>
            new GroupPermissionResolver(groups).aggregateRules();

        public IEnumerable<AccessRule> aggregateRulesForApp(string appName) =>
            new GroupPermissionResolver(groups).aggregateRulesForApp(appName);

        public class GroupPermissionResolver {
            private readonly IEnumerable<Group> groups;

            public GroupPermissionResolver(IEnumerable<Group> groups) {
                this.groups = groups;
            }

            public IEnumerable<Permission> aggregatePermissions() {
                return groups.SelectMany(x => x.permissions);
            }

            public List<AccessRule> aggregateRules() {
                // get all valid rules for the user]
                // this needs to take into account group priority
                // 1. sort group memberships
                var rankedGroups = groups.OrderByDescending(x => x.priority);
                // 2. take all unique rules
                var rules = new List<AccessRule>();
                foreach (var group in rankedGroups) {
                    foreach (var groupRule in group.rules) {
                        // ensure no rule with the same app and key exists
                        if (!rules.Any(x => x.app == groupRule.app && x.key == groupRule.key)) {
                            rules.Add(groupRule);
                        }
                    }
                }

                return rules;
            }

            public IEnumerable<AccessRule> aggregateRulesForApp(string appName) {
                return aggregateRules().Where(x => x.app == appName);
            }
        }
    }
}