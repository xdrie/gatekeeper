using System.ComponentModel.DataAnnotations.Schema;
using Gatekeeper.Config;
using Newtonsoft.Json;

namespace Gatekeeper.Models {
    public class DependencyObject {
        [JsonIgnore] [NotMapped] protected SContext serverContext { get; }

        protected DependencyObject(SContext context) {
            serverContext = context;
        }
    }
}