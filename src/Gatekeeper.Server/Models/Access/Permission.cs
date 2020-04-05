namespace Gatekeeper.Server.Models.Access {
    public class Permission : DatabaseObject {
        public string path { get; set; }

        public Permission(string path) {
            this.path = path;
        }
    }
}