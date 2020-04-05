using Gatekeeper.Models.Meta;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.OpenApi.Meta;
using Hexagon.Serialization;

namespace Gatekeeper.Server.Modules.Meta {
    public class MetaModule : ApiModule {
        public MetaModule(SContext serverContext) : base("/meta", serverContext) {
            Get<GetMeta>("/", async (req, res) => {
                await res.respondSerialized(new ServerMetadata());
            });
        }
    }
}