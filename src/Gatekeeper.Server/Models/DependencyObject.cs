using System.ComponentModel.DataAnnotations.Schema;
using Gatekeeper.Server.Config;
using Newtonsoft.Json;

namespace Gatekeeper.Server.Models {
    public class DependencyObject {
        [JsonIgnore] [NotMapped] protected SContext serverContext { get; }

        protected DependencyObject(SContext context) {
            serverContext = context;
        }
    }
}