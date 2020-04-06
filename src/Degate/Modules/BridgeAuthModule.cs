using System.Linq;
using System.Threading.Tasks;
using Degate.Config;
using Gatekeeper.Models.Identity;
using Hexagon;
using Hexagon.Modules;
using Hexagon.Security;
using Hexagon.Serialization;
using Hexagon.Services;

namespace Degate.Modules {
    public abstract class BridgeAuthModule<TContext> : ApiModule<TContext>
        where TContext : ServerContext, IDegateContext {
        public RemoteAuthentication remoteUser { get; private set; }

        protected BridgeAuthModule(string path, TContext serverContext) : base(path, serverContext) {
            // require authentication
            this.requiresUserAuthentication();

            Before += ctx => {
                // get the user
                var tokenClaim = ctx.User.Claims.First(x => x.Type == IBearerAuthenticator.CLAIM_TOKEN);
                remoteUser = serverContext.sessionTokenResolver.resolve(tokenClaim.Value);

                return Task.FromResult(true);
            };
        }
    }
}