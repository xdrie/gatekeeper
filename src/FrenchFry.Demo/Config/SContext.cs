using System;
using Degate.Config;
using Degate.Services;
using FrenchFry.Demo.Models;
using FrenchFry.Demo.Services;
using Gatekeeper.Remote;
using Hexagon;
using Hexagon.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FrenchFry.Demo.Config {
    public class SContext : ServerContext, IDegateContext {
        public const string GATE_APP = "FrenchFry";
        public const string GATE_SERVER = "http://localhost:5000";
        public const string GATE_SECRET = "yeet";

        public IAuthSessionResolver authSessionResolver { get; }
        public IRemoteAuthClient remoteAuthClient { get; }
        public UserManager userManager { get; }
        public override IBearerAuthenticator authenticator { get; }

        public SContext(IServiceCollection services) : base(services) {
            authSessionResolver = new AuthSessionResolver<SContext>(this);
            remoteAuthClient = new GateAuthClient(GATE_APP, new Uri(GATE_SERVER), GATE_SECRET);
            userManager = new UserManager(this);
            authenticator = new BearerAuthenticator<SContext>(this);
        }

        public AppDbContext getDbContext() => services.BuildServiceProvider().GetService<AppDbContext>();

        public override void start() {
            getDbContext().Database.EnsureCreated();
        }
    }
}