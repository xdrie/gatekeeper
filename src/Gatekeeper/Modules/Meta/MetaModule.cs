using Gatekeeper.Config;
using Gatekeeper.Models.Meta;
using Gatekeeper.OpenApi.Meta;
using Hexagon.Services.Serialization;

namespace Gatekeeper.Modules.Meta {
    public class MetaModule : ApiModule {
        public MetaModule(SContext serverContext) : base("/meta", serverContext) {
            Get<GetMeta>("/", async (req, res) => {
                await res.respondSerialized(new ServerMetadata());
            });
        }
    }
}