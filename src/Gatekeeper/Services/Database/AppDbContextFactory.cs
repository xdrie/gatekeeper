using Gatekeeper.Config;
using Gatekeeper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gatekeeper.Services.Database {
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext> {
        private SContext? serverContext { get; }

        public AppDbContextFactory(SContext context) {
            serverContext = context;
        }

        public AppDbContextFactory() { }

        public AppDbContext create() {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            if (serverContext != null) {
                builder.UseSqlite(serverContext.config.server.database);
                if (serverContext.config.logging.databaseLogging) {
                    builder.EnableDetailedErrors();
                    builder.EnableSensitiveDataLogging();
                }
            } else {
                builder.UseSqlite(SConfig.Server.DEFAULT_DATABASE);
            }

            return new AppDbContext(builder.Options);
        }

        public AppDbContext CreateDbContext(string[] args) {
            return this.create();
        }
    }
}