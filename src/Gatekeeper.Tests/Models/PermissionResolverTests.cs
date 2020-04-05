using System.Collections.Generic;
using Gatekeeper.Models.Access;
using Gatekeeper.Services.Users;
using Xunit;

namespace Gatekeeper.Tests.Models {
    public class PermissionResolverTests {
        public List<Group> groups = new List<Group>();
        private PermissionResolver.GroupPermissionResolver resolver;

        public PermissionResolverTests() {
            resolver = new PermissionResolver.GroupPermissionResolver(groups);
        }

        [Fact]
        public void canAggregateGroupPermissions() {
            var layer1Perm = new Permission("/Layer1");
            var layer2Perm = new Permission("/Layer2");
            groups.Add(new Group {
                name = "Test1",
                permissions = new List<Permission> {
                    layer1Perm
                }
            });
            groups.Add(new Group {
                name = "Test2",
                permissions = new List<Permission> {
                    layer2Perm
                }
            });
            Assert.Equal(new[] {layer1Perm, layer2Perm}, resolver.aggregatePermissions());
        }

        [Fact]
        public void canAggregateGroupRulesSimple() {
            var cakeRule = new AccessRule("Cake", "quota", "2");
            var brownieRule = new AccessRule("Brownie", "quota", "10");
            groups.Add(new Group {
                name = "Test1",
                rules = new List<AccessRule> {
                    cakeRule
                }
            });
            groups.Add(new Group {
                name = "Test2",
                rules = new List<AccessRule> {
                    brownieRule
                }
            });
            Assert.Equal(new[] {cakeRule, brownieRule}, resolver.aggregateRules());
        }

        [Fact]
        public void canAggregateGroupRulesPriority() {
            var lowCakeRule = new AccessRule("Cake", "quota", "2");
            var highCakeRule = new AccessRule("Cake", "quota", "20");
            groups.Add(new Group {
                name = "Home",
                priority = 2,
                rules = new List<AccessRule> {
                    lowCakeRule
                }
            });
            groups.Add(new Group {
                name = "Palace",
                priority = 10,
                rules = new List<AccessRule> {
                    highCakeRule
                }
            });
            Assert.Equal(new[] {highCakeRule}, resolver.aggregateRulesForApp("Cake"));
        }
    }
}