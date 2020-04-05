using System;
using System.IO;

namespace Gatekeeper.Server.Models.Identity {
    public struct AccessScope {
        public const string ROOT_PATH = "/";
        public const string WILDCARD_PATH = "*";

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
            if (path == ROOT_PATH) dir = ROOT_PATH; // check if root
            return new AccessScope(dir, file);
        }

        public string path => Path.Join(layer, app);

        public override string ToString() => path;

        /// <summary>
        /// whether this scope is greater than the given scope. for example, "/Scope" (this) is greater than "/Scope/App" (arg1)
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public bool greaterThan(AccessScope scope) {
            if (scope.layer == WILDCARD_PATH) return true; // anything is greater than the wildcard scope
            return scope.path.StartsWith(path);
        }

        public static AccessScope rootScope => new AccessScope(ROOT_PATH);
        public static AccessScope globalScope => new AccessScope(WILDCARD_PATH);
    }
}