using System.Collections.Generic;

namespace Gatekeeper.Server.Models.Remote {
    public class RemoteApp {
        public virtual string name { get; set; }
        public virtual List<string> layers { get; set; } = new List<string>();
        public virtual string secret { get; set; }
    }
}