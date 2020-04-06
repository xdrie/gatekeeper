using System;
using Degate.Modules;
using FrenchFry.Demo.Config;
using Hexagon.Modules;

namespace FrenchFry.Demo.Modules {
    public class AuthModule : ApiModule<SContext> {
        public AuthModule(SContext serverContext) : base("/auth", serverContext) {
            Get("/login", async (req, res) => {
                var gateCallbackUri = $"{req.Scheme}://{req.Host}/a/gate";
                res.Redirect(new Uri(serverContext.gateAuthClient.server,
                    $"/link?app={serverContext.gateAuthClient.app}&cb={gateCallbackUri}").ToString());
            });
        }
    }

    public class GateModule : GateLinkModule<SContext> {
        public GateModule(SContext serverContext) : base("/gate", serverContext) { }
    }
}