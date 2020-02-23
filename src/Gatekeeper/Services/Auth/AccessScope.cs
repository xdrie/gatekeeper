using System.IO;

namespace Gatekeeper.Services.Auth {
    public class AccessScope {
        public AccessScope() { }

        public const string rootPath = "/";

        public bool root;
        public string layer;
        public string app;

        public enum ScopeType {
            Root,
            Path
        }

        public AccessScope(string layer = null, string app = null) {
            if (layer == rootPath && app == null) {
                root = true;
            } else {
                this.layer = layer;
                this.app = app;
            }
        }

        public static AccessScope parse(string scope) {
            var res = new AccessScope();
            // check if path is root
            if (scope == rootPath) {
                // it is a root
                res.root = true;
            } else {
                res.layer = Path.GetDirectoryName(scope);
                var file = Path.GetFileName(scope);
                if (string.IsNullOrEmpty(file)) file = null;
                res.app = file;
            }

            return res;
        }

        public string pack() {
            if (root) return rootPath;
            else {
                return Path.Join(layer, app);
            }
        }
    }
}