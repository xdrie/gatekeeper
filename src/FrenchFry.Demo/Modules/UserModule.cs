using System.Linq;
using Degate.Modules;
using FrenchFry.Demo.Config;
using Hexagon.Modules;
using Hexagon.Serialization;

namespace FrenchFry.Demo.Modules {
    public class UserModule : GateAuthModule<SContext> {
        public UserModule(SContext serverContext) : base("/u", serverContext) {
            Get("/me", async (req, res) => {
                // display user info
                await res.respondSerialized(remoteUser);
            });
            Get("/status", async (req, res) => {
                var fryQuota = long.Parse(
                    remoteUser.rules.SingleOrDefault(x => x.key == "quota")?.value ?? "0");
                await res.respondSerialized($"hello {remoteUser.user.name}! you have {fryQuota} monthly fries!");
            });
        }
    }
}