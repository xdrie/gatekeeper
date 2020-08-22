using System;
using Gatekeeper.Server.Models;
using Gatekeeper.Server.Services.Auth;
using Gatekeeper.Server.Services.Users;
using Hexagon;
using Hexagon.Services;
using Iri.Glass.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Gatekeeper.Server.Config {
    public class SContext : ServerContext {
        /// <summary>
        /// configuration used to create this context instance
        /// </summary>
        public SConfig config { get; }
        public AppDbContext getDbContext() => services.BuildServiceProvider().GetService<AppDbContext>();
        public UserManagerService userManager { get; set; }
        public TokenAuthenticationService tokenResolver { get; set; }
        public override IBearerAuthenticator authenticator { get; }

        public SContext(IServiceCollection services, SConfig config) : base(services) {
            this.config = config;
            log.verbosity = config.logging.Verbosity;
            userManager = new UserManagerService(this);
            tokenResolver = new TokenAuthenticationService(this);
            authenticator = new BearerAuthenticator(this);
        }

        public override void start() {
            base.start();

            tickService.schedule(() => {
                var prunedTokens = tokenResolver.pruneExpiredTokens();
                log.writeLine($"pruned {prunedTokens} expired tokens.",
                    Logger.Verbosity.Information);
            }, TimeSpan.FromHours(8));
        }

        public override void Dispose() {
            base.Dispose();
        }
    }
}