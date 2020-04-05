using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carter;
using Gatekeeper.Models;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Models;
using Gatekeeper.Server.OpenApi;
using Hexagon.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tomlyn;

namespace Gatekeeper.Server {
    public class Startup {
        private const string CONFIG_FILE = "config.toml";

        public void ConfigureServices(IServiceCollection services) {
            // Adds services required for using options.
            services.AddOptions();
            // install Carter
            services.AddCarter(options => {
                options.OpenApi.DocumentTitle = SConfig.SERVER_NAME;
                options.OpenApi.ServerUrls = new[] {"http://localhost:5000"};
                options.OpenApi.Securities = new Dictionary<string, OpenApiSecurity> {
                    {
                        GateApiConstants.Security.USER_BEARER_AUTH,
                        new OpenApiSecurity {Type = OpenApiSecurityType.http, Scheme = "bearer"}
                    }, {
                        GateApiConstants.Security.REMOTE_APP_APIKEY,
                        new OpenApiSecurity
                            {Type = OpenApiSecurityType.apiKey, In = OpenApiIn.header, Name = "X-App-Secret"}
                    },
                    // { "ApiKey", new OpenApiSecurity { Type = OpenApiSecurityType.apiKey, Name = "X-API-KEY", In = OpenApiIn.header } }
                };
                options.OpenApi.GlobalSecurityDefinitions = new[] {
                    GateApiConstants.Security.USER_BEARER_AUTH,
                    GateApiConstants.Security.REMOTE_APP_APIKEY
                };
            });

            var serverConfig = default(SConfig);
            var configDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(SConfig));
            if (configDescriptor == null) {
                // load configuration
                serverConfig = new SConfig(); // default configuration
                if (File.Exists(CONFIG_FILE)) {
                    var configTxt = File.ReadAllText(CONFIG_FILE);
                    var configDoc = Toml.Parse(configTxt);
                    var configModel = configDoc.ToModel();
                    serverConfig = ConfigLoader.readDocument(configModel);
                }
            }
            else {
                serverConfig = (SConfig) configDescriptor.ImplementationInstance;
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

                // show banner and log some server information
                SBoot.display(serverContext);
#if DEBUG
                // print debug banner (always)
                serverContext.log.writeLine(
                    $"this is a DEBUG build of {nameof(Server)}. this build should NEVER be used in production.",
                    ConsoleColor.Red);
                if (serverContext.config.server.development) {
                    serverContext.log.writeLine(
                        $"development/test mode is enabled. default values and fake external services will be used.",
                        ConsoleColor.Red);
                }
#else
                if (env.IsProduction()) {
                    serverContext.log.writeLine(
                        $"this is a release build of {nameof(Gatekeeper.Server)}, but is not being run in PRODUCTION (it is being run in '{env.EnvironmentName}')",
                        ConsoleColor.Red);
                }
#endif

                var configuredAppNames = string.Join(", ", serverContext.config.apps.Select(x => x.name));
                serverContext.log.writeLine(
                    $"available remote application configurations[{serverContext.config.apps.Count}]: remote applications are configured: [{configuredAppNames}]",
                    SLogger.LogLevel.Information);

                app.UseRouting();

                app.UseEndpoints(builder => builder.MapCarter());
                app.UseSwaggerUi3(settings => { settings.DocumentPath = "/openapi"; });
            }
        }
    }
}