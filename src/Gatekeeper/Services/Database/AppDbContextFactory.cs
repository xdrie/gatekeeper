using Gatekeeper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gatekeeper.Services.Database {
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext> {
        public AppDbContext create() {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlite("Data Source=database.db");
            // if (serverContext.config.logging.databaseLogging) {
            builder.EnableDetailedErrors();
            builder.EnableSensitiveDataLogging();
            // }

            return new AppDbContext(builder.Options);
        }

        public AppDbContext CreateDbContext(string[] args) {
            return this.create();
        }
    }
}