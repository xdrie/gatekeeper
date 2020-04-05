using System.Collections.Generic;

namespace Gatekeeper.Models.Access {
    public class Group {
        public string name { get; set; }
        public List<Permission> permissions { get; set; } = new List<Permission>();
    }
}