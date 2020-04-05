using System.Collections.Generic;

namespace Gatekeeper.Models.Access {
    public class PermissionList {
        public List<string> paths { get; set; } = new List<string>();

        public PermissionList(IEnumerable<Permission> permissions) {
            foreach (var perm in permissions) {
                paths.Add(perm.path);
            }
        }
    }
}