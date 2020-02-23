using System;
using Gatekeeper.Models;
using Gatekeeper.Services.Users;
using Hexagon.Services.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Gatekeeper.Config {
    public class SContext : IDisposable {
        /// <summary>
        /// configuration used to create this context instance
        /// </summary>
        public SConfig config { get; }
        
        public IServiceCollection services { get; set; }
        public IServiceProvider serviceProvider { get; set; }
        
        public AppDbContext dbContext => serviceProvider.GetService<AppDbContext>();
        public UserManagerService userManager { get; set; }

        public SLogger log;

        public SContext(IServiceCollection services, SConfig config) {
            this.config = config;
            log = new SLogger(config.logging.logLevel);
            userManager = new UserManagerService(this);
            this.services = services;
        }

        public void build() {
            serviceProvider = services.BuildServiceProvider();
        }

        public void Dispose() { }
    }
}