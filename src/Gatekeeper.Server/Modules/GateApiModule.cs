using System.Threading.Tasks;
using Gatekeeper.Server.Config;
using Hexagon.Modules;

namespace Gatekeeper.Server.Modules {
    public abstract class GateApiModule : ApiModule<SContext> {
        public GateApiModule(string path, SContext serverContext) : base(path, serverContext) {
            Before += ctx => {
                var origin = ctx.Request.Headers["Origin"];
                if (serverContext.config.server.cors.Contains(origin)) {
                    ctx.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                }

                return Task.FromResult(true);
            };
        }
    }
}