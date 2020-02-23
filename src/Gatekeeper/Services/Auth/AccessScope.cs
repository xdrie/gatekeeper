using System.IO;

namespace Gatekeeper.Services.Auth {
    public struct AccessScope {
        public const string ROOT_PATH = "/";

        public string layer;
        public string app;

        public AccessScope(string layer, string app) {
            this.layer = layer;
            this.app = app;
        }

        public AccessScope(string layer) : this(layer, string.Empty) { }

        public static AccessScope parse(string path) {
            var dir = Path.GetDirectoryName(path);
            var file = Path.GetFileName(path);
            return new AccessScope(dir, file);
        }

        public string path => Path.Join(layer, app);
    }
}