namespace Gatekeeper.Models.Identity {
    public struct TokenCredential {
        public Token token;
        public AccessScope scope;

        public TokenCredential(Token token, AccessScope scope) {
            this.token = token;
            this.scope = scope;
        }
    }
}