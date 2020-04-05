#region

using System;
using System.ComponentModel.DataAnnotations;
using MsgPack.Serialization;
using Newtonsoft.Json;

#endregion

namespace Gatekeeper.Server.Models {
    public class DatabaseObject {
        [JsonIgnore]
        [MessagePackIgnore]
        [Key]
        public int dbid { get; set; }
    }
}