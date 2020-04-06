using Gatekeeper.Models.Identity;

namespace Gatekeeper.Remote {
    public class GateUser {
        public RemoteAuthentication auth { get; }
        public string session { get; }
        
        public GateUser(RemoteAuthentication auth, string session) {
            this.auth = auth;
            this.session = session;
        }
    }
}