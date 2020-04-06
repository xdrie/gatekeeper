using System;
using Gatekeeper.Models;
using Gatekeeper.Server.Models;
using Gatekeeper.Server.Services.Auth;
using Gatekeeper.Server.Services.Users;
using Hexagon;
using Hexagon.Logging;
using Hexagon.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gatekeeper.Server.Config {
    public class SContext : ServerContext {
        /// <summary>
        /// configuration used to create this context instance
        /// </summary>
        public SConfig config { get; }

        public IServiceCollection services { get; set; }
        public AppDbContext getDbContext() => services.BuildServiceProvider().GetService<AppDbContext>();
        public UserManagerService userManager { get; set; }
        public TokenAuthenticationService tokenResolver { get; set; }

        public SContext(IServiceCollection services, SConfig config) {
            this.config = config;
            log.verbosity = config.logging.logLevel;
            userManager = new UserManagerService(this);
            tokenResolver = new TokenAuthenticationService(this);
            this.services = services;
        }

        public override IBearerAuthenticator getAuthenticator() => new BearerAuthenticator(this);

        public override void Dispose() {
            base.Dispose();
        }
    }
}