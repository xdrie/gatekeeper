#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Remote;
using Gatekeeper.Server;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Models;
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
                    var testConfig = createTestConfig();
                    services.AddSingleton(testConfig);

                    // Build the service provider.
                    var sp = services.BuildServiceProvider();
                    // serverContext = sp.GetService<SContext>();
                    //
                    // // set server context options
                    // serverContext.log.verbosity = Logger.Verbosity.Trace;

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

            private SConfig createTestConfig() {
                var testConfig = new SConfig();

                // - apps
                testConfig.apps.Add(new RemoteApp {
                    name = "Hotels",
                    layers = new List<string> {"/Housing"},
                    secret = Constants.Apps.APP_SECRET
                });
                testConfig.apps.Add(new RemoteApp {
                    name = "Salt",
                    layers = new List<string> {"/Food"},
                    secret = Constants.Apps.APP_SECRET
                });

                // - groups
                testConfig.groups.Add(new Group {
                    name = "Luxurious",
                    permissions = {
                        new Permission("/Housing")
                    }
                });
                testConfig.groups.Add(new Group {
                    name = "Friends",
                    permissions = {
                        new Permission("/Food")
                    }
                });

                testConfig.users.defaultGroups = new List<string> {"Friends"};

                return testConfig;
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