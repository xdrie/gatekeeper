using System.IO;
using Gatekeeper.Config;
using Tomlyn;

namespace Gatekeeper {
    using Carter;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

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

            var context = new SContext(serverConfig);
            // register server context
            services.AddSingleton(context);
        }

        public void Configure(IApplicationBuilder app) {
            app.UseRouting();

            app.UseEndpoints(builder => builder.MapCarter());
            app.UseSwaggerUi3(settings => { settings.DocumentPath = "/openapi"; });
        }
    }
}