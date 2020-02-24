using Gatekeeper.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gatekeeper.Models {
    public class AppDbContext : DbContext {
        public DbSet<User> users { get; set; }
        public DbSet<Token> tokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>()
                .HasIndex(p => new {p.username, p.email, p.uuid})
                .IsUnique();
        }
    }
}