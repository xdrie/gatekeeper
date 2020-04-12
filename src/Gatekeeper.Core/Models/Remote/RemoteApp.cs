using System.Collections.Generic;

namespace Gatekeeper.Models.Remote {
    public class RemoteApp {
        public string name;
        public List<string> layers = new List<string>();
        public string secret;
    }
}