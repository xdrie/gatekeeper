using System.IO;
using Carter;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tomlyn;

namespace Gatekeeper {
    public class Startup {
        private const string CONFIG_FILE = "config.toml";

        public void ConfigureServices(IServiceCollection services) {
            // Adds services required for using options.
            services.AddOptions();
            // install Carter
            services.AddCarter(options => {
                options.OpenApi.DocumentTitle = "ALTiCU Unified Authentication Service";
                options.OpenApi.ServerUrls = new[] {"http://localhost:5000"};
            });

            // load configuration
            var serverConfig = new SConfig(); // default configuration
            if (File.Exists(CONFIG_FILE)) {
                var configTxt = File.ReadAllText(CONFIG_FILE);
                var configDoc = Toml.Parse(configTxt);
                var configModel = configDoc.ToModel();
                serverConfig = ConfigLoader.readDocument(configModel);
            }

            var context = new SContext(services, serverConfig);
            // register server context
            services.AddSingleton(context);

            // set up database
            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlite(context.config.server.database);
                // options.UseInMemoryDatabase("InMemoryDb");
                if (context.config.logging.databaseLogging) {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }
            });

            // server context start signal
            // context.build();
        }

        public void Configure(IApplicationBuilder app) {
            // update the database
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope()) {
                
                var serverContext = app.ApplicationServices.GetService<SContext>();
                using (var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>()) {
                    if (!dbContext.Database.IsInMemory()) {
                        dbContext.Database.Migrate();
                    }
                    // context.Database.EnsureCreated();
                }
            }

            app.UseRouting();

            app.UseEndpoints(builder => builder.MapCarter());
            app.UseSwaggerUi3(settings => { settings.DocumentPath = "/openapi"; });
        }
    }
}