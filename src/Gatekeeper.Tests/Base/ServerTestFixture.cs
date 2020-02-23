#region

using System;
using System.Linq;
using System.Net.Http;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Services.Database;
using Hexagon.Services.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace Gatekeeper.Tests.Base {
    public class ServerTestFixture : IDisposable {
        public CustomWebApplicationFactory<Startup> factory { get; }

        public class CustomWebApplicationFactory<TStartup>
            : WebApplicationFactory<TStartup> where TStartup : class {
            protected override void ConfigureWebHost(IWebHostBuilder builder) {
                builder.ConfigureServices(services => {
                    // Remove the app's ApplicationDbContext registration.
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

                    // Build the service provider.
                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database
                    // context (ApplicationDbContext).
                    using (var scope = sp.CreateScope()) {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<AppDbContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                        // Ensure the database is created.
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