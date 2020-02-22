namespace Gatekeeper {
    using Carter;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddCarter(options => {
                options.OpenApi.DocumentTitle = "ALTiCU Unified Authentication Service";
                options.OpenApi.ServerUrls = new[] {"http://localhost:5000"};
            });
        }

        public void Configure(IApplicationBuilder app) {
            app.UseRouting();

            app.UseEndpoints(builder => builder.MapCarter());
            app.UseSwaggerUi3(settings => { settings.DocumentPath = "/openapi"; });
        }
    }
}