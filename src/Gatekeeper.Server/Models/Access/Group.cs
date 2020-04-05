using System.Collections.Generic;

namespace Gatekeeper.Server.Models.Access {
    public class Group {
        public string name { get; set; }
        public long priority { get; set; } = 0;
        public List<Permission> permissions { get; set; } = new List<Permission>();
        public List<AccessRule> rules { get; set; } = new List<AccessRule>();
        
        public enum UpdateType {
            Add,
            Remove
        }
    }
}