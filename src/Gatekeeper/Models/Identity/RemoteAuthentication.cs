using System.Collections.Generic;
using Gatekeeper.Models.Access;

namespace Gatekeeper.Models.Identity {
    public class RemoteAuthentication {
        public PublicUser user { get; }
        public IEnumerable<AccessRule> rules { get; }
        
        public RemoteAuthentication(PublicUser user, IEnumerable<AccessRule> rules) {
            this.user = user;
            this.rules = rules;
        }
    }
}