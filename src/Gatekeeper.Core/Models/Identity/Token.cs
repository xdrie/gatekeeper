using System;
using MsgPack.Serialization;
using Newtonsoft.Json;

namespace Gatekeeper.Models.Identity {
    public class Token : DatabaseObject {
        [JsonIgnore]
        [MessagePackIgnore]
        public User user { get; set; }
        public string content { get; set; }
        public DateTime expires { get; set; }
        public string scope { get; set; }

        public bool expired() => expires <= DateTime.UtcNow;
    }
}