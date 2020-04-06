using Microsoft.EntityFrameworkCore;

namespace FrenchFry.Demo.Models {
    public class AppDbContext : DbContext {
        public DbSet<Account> accounts { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Account>()
                .HasIndex(p => p.userId)
                .IsUnique();
        }
    }
}