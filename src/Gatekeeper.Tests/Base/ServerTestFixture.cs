#region

using System;
using System.Linq;
using System.Net.Http;
using Gatekeeper.Config;
using Hexagon.Services.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

#endregion

namespace Gatekeeper.Tests.Base {
    public class ServerTestFixture : IDisposable {
        public TestSpeercsApplicationFactory<Startup> factory { get; }
        public SContext serverContext => factory.serverContext;

        public class TestSpeercsApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class {
            public SContext serverContext;

            protected override void ConfigureWebHost(IWebHostBuilder builder) {
                builder.ConfigureTestServices(services => {
                    var contextDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(SContext));
                    if (contextDescriptor == null) throw new ApplicationException("server context service is missing");
                    serverContext = (SContext) contextDescriptor.ImplementationInstance;

                    // -- update configuration for tests
                    // transient database
                    serverContext.config.server.database = null;
                    // verbose logging
                    serverContext.log.verbosity = SLogger.LogLevel.Trace;
                });
            }
        }

        public ServerTestFixture() {
            factory = new TestSpeercsApplicationFactory<Startup>();
        }

        public HttpClient getClient() {
            return factory.CreateClient();
        }

        public void Dispose() {
            factory.Dispose();
        }
    }
}