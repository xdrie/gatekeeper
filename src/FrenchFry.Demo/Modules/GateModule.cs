using FrenchFry.Demo.Config;
using Hexagon.Modules;

namespace FrenchFry.Demo.Modules {
    public class GateModule : ApiModule<SContext> {
        public GateModule(SContext serverContext) : base("/gate", serverContext) {
            Post("/", async (req, res) => {
                // TODO: token is in form data
            });
        }
    }
}