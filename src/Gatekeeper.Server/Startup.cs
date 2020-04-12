using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carter;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Models;
using Gatekeeper.Server.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Gatekeeper.Server {
    public class Startup {
        private const string CONFIG_FILE = "config.toml";

        public Startup(IConfiguration aspnetConfig, IWebHostEnvironment hostEnv) {
            this.aspnetConfig = aspnetConfig;
            this.hostEnv = hostEnv;
        }

        public IConfiguration aspnetConfig { get; }
        public IWebHostEnvironment hostEnv { get; }

        public void ConfigureServices(IServiceCollection services) {
            // Adds services required for using options.
            services.AddOptions();

            // enable Razor Pages
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

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

            var confBuilder = new ConfigurationBuilder()
                .AddIniFile(Path.Combine(Directory.GetCurrentDirectory(), "server.cfg"), optional: true)
                .AddEnvironmentVariables(prefix: "GATEKEEPER_")
                .AddCommandLine(Environment.GetCommandLineArgs().Skip(1).ToArray());
            var conf = confBuilder.Build();

            var serverConfig = new SConfig();
            var configDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(SConfig));
            if (configDescriptor == null)
                conf.Bind(serverConfig);
            else
                serverConfig = (SConfig) configDescriptor.ImplementationInstance;

            var context = new SContext(services, serverConfig);
            // register server context
            services.AddSingleton(context);

            // set up database
            services.AddDbContext<AppDbContext>(options => {
                switch (serverConfig.server.databaseBackend) {
                    case SConfig.Server.DatabaseBackend.InMemory:
                        options.UseInMemoryDatabase(context.config.server.databaseConnection);
                        break;
                    case SConfig.Server.DatabaseBackend.Sqlite:
                        options.UseSqlite(context.config.server.databaseConnection);
                        break;
                    case SConfig.Server.DatabaseBackend.Postgres:
                        options.UseNpgsql(context.config.server.databaseConnection);
                        break;
                }
                if (context.config.logging.databaseLogging) {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }
            });

            // Make our logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(conf)
                .WriteTo.Console()
                .CreateLogger();

            // server context start signal
            context.start();
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

                // show banner and log some server information
                SBoot.display(serverContext, hostEnv);
            }

            if (hostEnv.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapCarter());
            app.UseEndpoints(endpoints => endpoints.MapRazorPages());
            app.UseSwaggerUi3(settings => settings.DocumentPath = "/openapi");
        }
    }
}