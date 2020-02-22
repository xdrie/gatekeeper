#region

using System.ComponentModel.DataAnnotations.Schema;
using Gatekeeper.Config;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#endregion

namespace Gatekeeper.Models {
    public class DependencyDbContext : DbContext {
        [JsonIgnore] [NotMapped] protected SContext serverContext { get; }

        protected DependencyDbContext(SContext context) {
            serverContext = context;
        }
    }
}