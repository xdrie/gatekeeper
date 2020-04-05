namespace Gatekeeper.Server.Models.Identity {
    public struct Credential {
        public Token token;
        public AccessScope scope;

        public Credential(Token token, AccessScope scope) {
            this.token = token;
            this.scope = scope;
        }
    }
}