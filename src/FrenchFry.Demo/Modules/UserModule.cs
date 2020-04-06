using Degate.Modules;
using Degate.Utilities;
using FrenchFry.Demo.Config;
using Hexagon.Serialization;

namespace FrenchFry.Demo.Modules {
    public class UserModule : BridgeAuthModule<SContext> {
        public UserModule(SContext serverContext) : base("/u", serverContext) {
            Get("/status", async (req, res) => {
                remoteUser.rules
                    .getAppRule<long>(SContext.GATE_APP, "quota", 0, out var fryQuota);
                await res.respondSerialized($"hello {remoteUser.user.name}! you have {fryQuota} monthly fries!");
            });
        }
    }
}