#region

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

#endregion

namespace Gatekeeper.Models {
    public class DatabaseObject {
        [JsonIgnore] [Key] public int dbid { get; set; }
    }
}