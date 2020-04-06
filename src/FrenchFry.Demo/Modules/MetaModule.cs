using Carter.Response;
using FrenchFry.Demo.Config;
using Hexagon.Modules;

namespace FrenchFry.Demo.Modules {
    public class MetaModule : ApiModule<SContext> {
        public MetaModule(SContext serverContext) : base("/meta", serverContext) {
            Get("/", async (req, res) => { await res.Negotiate("frenchfry server"); });
        }
    }
}