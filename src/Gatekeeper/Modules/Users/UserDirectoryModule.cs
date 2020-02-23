using Gatekeeper.Config;

namespace Gatekeeper.Modules.Users {
    public class UserDirectoryModule : ApiModule {
        public UserDirectoryModule(SContext serverContext) : base("/u", serverContext) { }
    }
}