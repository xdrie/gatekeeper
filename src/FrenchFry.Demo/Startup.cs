using Carter;
using FrenchFry.Demo.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FrenchFry.Demo {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddCarter();

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            
            var context = new SContext();
            // register server context
            services.AddSingleton(context);
        }

        public void Configure(IApplicationBuilder app) {
            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(builder => builder.MapCarter());
            app.UseEndpoints(builder => builder.MapRazorPages());
        }
    }
}