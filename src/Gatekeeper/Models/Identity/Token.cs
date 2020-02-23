using System;

namespace Gatekeeper.Models.Identity {
    public class Token : DatabaseObject {
        public string content { get; set; }
        public User user { get; set; }
        public DateTime expires { get; set; }
        public string scope { get; set; }
    }
}