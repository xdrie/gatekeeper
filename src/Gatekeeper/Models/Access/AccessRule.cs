namespace Gatekeeper.Models.Access {
    public class AccessRule {
        public string app;
        public string key;
        public string value;
        public AccessRule(string app, string key, string value) {
            this.app = app;
            this.key = key;
            this.value = value;
        }
    }
}