using Gatekeeper.Config;
using Microsoft.AspNetCore.Http;

namespace Gatekeeper.Modules {
    public class HomeModule : ApiModule {
        public HomeModule(SContext context) : base("/", context) {
            Get("/", async (req, res) => await res.WriteAsync("Hello from Carter!"));
        }
    }
}