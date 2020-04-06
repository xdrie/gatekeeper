using Gatekeeper.Models.Meta;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.OpenApi.Meta;
using Hexagon.Modules;
using Hexagon.Serialization;

namespace Gatekeeper.Server.Modules.Meta {
    public class MetaModule : ApiModule<SContext> {
        public MetaModule(SContext serverContext) : base("/meta", serverContext) {
            Get<GetMeta>("/", async (req, res) => {
                await res.respondSerialized(new ServerMetadata {
                    name = SConfig.SERVER_NAME,
                    version = SConfig.VERSION
                });
            });
        }
    }
}