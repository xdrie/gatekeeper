using Degate.Config;
using Hexagon;
using Hexagon.Serialization;

namespace Degate.Modules {
    public abstract class BridgeUserInfoModule<TContext> : BridgeAuthModule<TContext>
        where TContext : ServerContext, IDegateContext {
        protected BridgeUserInfoModule(string path, TContext serverContext) : base(path, serverContext) {
            Get("/", async (req, res) => {
                await res.respondSerialized(remoteUser); // user info
            });
        }
    }
}