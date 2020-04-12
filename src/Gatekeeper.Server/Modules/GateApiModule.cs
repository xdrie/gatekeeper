using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Server.Config;
using Hexagon.Modules;

namespace Gatekeeper.Server.Modules {
    public abstract class GateApiModule : ApiModule<SContext> {
        public GateApiModule(string path, SContext serverContext) : base(path, serverContext) {
        }
    }
}