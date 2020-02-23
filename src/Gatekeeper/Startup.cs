using System.Collections.Generic;
using System.IO;
using Carter;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.OpenApi;
using Hexagon.Services.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                options.OpenApi.Securities = new Dictionary<string, OpenApiSecurity> {
                    {
                        GateApiConstants.Security.USER_BEARER_AUTH,
                        new OpenApiSecurity {Type = OpenApiSecurityType.http, Scheme = "bearer"}
                    },
                    // { "ApiKey", new OpenApiSecurity { Type = OpenApiSecurityType.apiKey, Name = "X-API-KEY", In = OpenApiIn.header } }
                };
                options.OpenApi.GlobalSecurityDefinitions = new[] {GateApiConstants.Security.USER_BEARER_AUTH};
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
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

                // log some server information
                serverContext.log.writeLine(
                    $"running {nameof(Gatekeeper)} v{SConfig.VERSION} instance '{SConfig.SERVER_NAME}'",
                    SLogger.LogLevel.Information);

                // print host information
#if DEBUG
                // print debug banner (always)
                serverContext.log.writeLine(
                    $"this is a DEBUG build of {nameof(Gatekeeper)}. this build should NEVER be used in production.",
                    SLogger.LogLevel.Warning);
                if (serverContext.config.server.development) {
                    serverContext.log.writeLine(
                        $"development/test mode is enabled. default values and fake external services will be used.",
                        SLogger.LogLevel.Warning);
                }
#else
                if (env.IsProduction()) {
                    serverContext.log.writeLine(
                        $"this is a release build of {nameof(Gatekeeper)}, but is not being run in PRODUCTION (it is being run in '{env.EnvironmentName}')",
                        SLogger.LogLevel.Warning);
                }
#endif
                app.UseRouting();

                app.UseEndpoints(builder => builder.MapCarter());
                app.UseSwaggerUi3(settings => { settings.DocumentPath = "/openapi"; });
            }
        }
    }
}