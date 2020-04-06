using Microsoft.EntityFrameworkCore;

namespace FrenchFry.Demo.Models {
    public class AppDbContext : DbContext {
        public DbSet<User> users { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>()
                .HasIndex(p => p.userId)
                .IsUnique();
        }
    }
}