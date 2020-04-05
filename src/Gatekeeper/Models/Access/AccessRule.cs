namespace Gatekeeper.Models.Access {
    public class AccessRule {
        public string app { get; }
        public string key { get; }
        public string value { get; }

        public AccessRule(string app, string key, string value) {
            this.app = app;
            this.key = key;
            this.value = value;
        }
    }
}