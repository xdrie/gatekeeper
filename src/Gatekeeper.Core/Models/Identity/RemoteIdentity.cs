namespace Gatekeeper.Models.Identity {
    public class RemoteIdentity {
        public PublicUser user { get; }
        public Token token { get; }
        
        public RemoteIdentity(PublicUser user, Token token) {
            this.user = user;
            this.token = token;
        }
    }
}