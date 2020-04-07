#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MsgPack.Serialization;
using Newtonsoft.Json;

#endregion

namespace Gatekeeper.Models {
    public class DatabaseObject {
        [JsonIgnore]
        [MessagePackIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
    }
}