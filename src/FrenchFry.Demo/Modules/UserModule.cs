using FrenchFry.Demo.Config;
using Hexagon.Modules;
using Hexagon.Serialization;

namespace FrenchFry.Demo.Modules {
    public class UserModule : AuthenticatedModule {
        public UserModule(SContext serverContext) : base("/u", serverContext) {
            Get("/me", async (req, res) => {
                // display user info
                await res.respondSerialized(user);
            });
        }
    }
}