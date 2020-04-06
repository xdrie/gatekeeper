using Gatekeeper.Models.Identity;
using Hexagon;
using Hexagon.Models;

namespace Degate.Services {
    public class SessionTokenResolver<TContext> : DependencyService<TContext>, IRemoteTokenResolver where TContext : ServerContext {
        public SessionTokenResolver(TContext context) : base(context) { }

        public RemoteAuthentication resolve(string token) {
            // resolve user by token

            // 1. get matching session
            if (!serverContext.sessions.exists(token)) return null;

            var sess = serverContext.sessions.get(token);
            var user = sess.jar.Resolve<RemoteAuthentication>();
            return user;
        }
    }
}