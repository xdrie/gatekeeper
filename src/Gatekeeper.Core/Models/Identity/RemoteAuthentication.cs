using System.Collections.Generic;
using System.Linq;
using Gatekeeper.Models.Access;

namespace Gatekeeper.Models.Identity {
    public class RemoteAuthentication {
        public PublicUser user { get; }
        public List<AccessRule> rules { get; }
        
        public RemoteAuthentication(PublicUser user, IEnumerable<AccessRule> rules) {
            this.user = user;
            this.rules = rules.ToList();
        }
    }
}