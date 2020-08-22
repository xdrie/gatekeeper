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

            // Adds services required for using CORS
            services.AddCors();

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

            var serverConfig = default(SConfig);
            var configDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(SConfig));
            if (configDescriptor == null) {
                // load configuration
                serverConfig = new SConfig(); // default configuration
                if (File.Exists(CONFIG_FILE)) {
                    var configTxt = File.ReadAllText(CONFIG_FILE);
                    serverConfig.load(configTxt);
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

            // server context start signal
            context.start();
        }

        public void Configure(IApplicationBuilder app) {
            // update the database
            var serverContext = app.ApplicationServices.GetService<SContext>();
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope()) {

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
            app.UseCors(b => {
                b
                    .WithOrigins(serverContext.config.server.cors.ToArray())
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
            app.UseEndpoints(endpoints => endpoints.MapCarter());
            app.UseEndpoints(endpoints => endpoints.MapRazorPages());
            app.UseSwaggerUi3(settings => settings.DocumentPath = "/openapi");
        }
    }
}