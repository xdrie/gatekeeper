namespace Gatekeeper.Models.Access {
    public class Permission : DatabaseObject {
        public string path { get; set; }

        public Permission(string path) {
            this.path = path;
        }

        public enum PermissionUpdateType {
            Add,
            Remove
        }
    }
}