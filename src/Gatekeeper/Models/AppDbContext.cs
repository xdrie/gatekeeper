using Gatekeeper.Config;

namespace Gatekeeper.Models {
    public class AppDbContext : DependencyDbContext {
        protected AppDbContext(SContext context) : base(context) { }
    }
}