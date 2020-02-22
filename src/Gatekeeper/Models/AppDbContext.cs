using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gatekeeper.Models {
    public class AppDbContext : DependencyDbContext {
        public AppDbContext(SContext context) : base(context) { }

        public DbSet<User> users { get; set; }
        public DbSet<Token> tokens { get; set; }
    }
}