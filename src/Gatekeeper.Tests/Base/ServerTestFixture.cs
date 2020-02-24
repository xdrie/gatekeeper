#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace Gatekeeper.Tests.Base {
    public class ServerTestFixture : IDisposable {
        public TestWebApplicationFactory<Startup> factory { get; }
        public SContext serverContext => factory.Server.Services.GetService<SContext>();

        public class TestWebApplicationFactory<TStartup>
            : WebApplicationFactory<TStartup> where TStartup : class {
            // public SContext serverContext;

            protected override void ConfigureWebHost(IWebHostBuilder builder) {
                builder.ConfigureServices(services => {
                    // remove the existing database registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null) {
                        services.Remove(descriptor);
                    }

                    // Add ApplicationDbContext using an in-memory database for testing.
                    services.AddDbContext<AppDbContext>(options => {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                    
                    // add test config
                    var testConfig = new SConfig();
                    testConfig.apps.Add(new SConfig.RemoteApp {
                        name = "BeanCan",
                        scopes = new List<string> {"/ExpensiveFood/BeanCan"}
                    });
                    testConfig.apps.Add(new SConfig.RemoteApp {
                        name = "SaltShaker",
                        scopes = new List<string> {"/CheapFood/SaltShaker"}
                    });
                    services.AddSingleton<SConfig>(testConfig);

                    // Build the service provider.
                    var sp = services.BuildServiceProvider();
                    // serverContext = sp.GetService<SContext>();
                    //
                    // // set server context options
                    // serverContext.log.verbosity = SLogger.LogLevel.Trace;

                    // Create a scope to obtain a reference to the database
                    // context (ApplicationDbContext).
                    using (var scope = sp.CreateScope()) {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<AppDbContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();

                        // Ensure the in-memory database is created.
                        db.Database.EnsureCreated();

                        try {
                            // Seed the database with test data.
                            // Utilities.InitializeDbForTests(db);
                        }
                        catch (Exception ex) {
                            logger.LogError(ex, "An error occurred seeding the " +
                                                "database with test data. Error: {Message}", ex.Message);
                        }
                    }
                });
            }
        }

        public ServerTestFixture() {
            factory = new TestWebApplicationFactory<Startup>();
        }

        public HttpClient getClient() {
            return factory.CreateClient();
        }

        public void Dispose() {
            factory.Dispose();
        }
    }
}