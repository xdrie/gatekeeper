using Gatekeeper.Server.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gatekeeper.Server.Models {
    public class AppDbContext : DbContext {
        public DbSet<User> users { get; set; }
        public DbSet<Token> tokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>()
                .HasIndex(p => p.username)
                .IsUnique();
            builder.Entity<User>()
                .HasIndex(p => p.email)
                .IsUnique();
            builder.Entity<User>()
                .HasIndex(p => p.uuid)
                .IsUnique();

            builder.Entity<Token>()
                .HasIndex(x => x.content)
                .IsUnique();
        }
    }
}