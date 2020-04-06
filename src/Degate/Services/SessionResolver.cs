using System;
using Gatekeeper.Models.Identity;
using Hexagon;
using Hexagon.Models;
using Hexagon.Utilities;

namespace Degate.Services {
    public class SessionResolver<TContext> : DependencyService<TContext>, ISessionResolver
        where TContext : ServerContext {

        public string cryptKey { get; }
        
        public SessionResolver(TContext context) : base(context) {
            cryptKey = Convert.ToBase64String(AesCrypt.randomBytes(AesCrypt.KEY_LENGTH));
        }

        public virtual RemoteAuthentication resolveSessionToken(string token) {
            // resolve user by token

            // 1. get matching session
            if (!serverContext.sessions.exists(token)) return null;

            var sess = serverContext.sessions.get(token);
            var user = sess.jar.Resolve<RemoteAuthentication>();
            return user;
        }

        public virtual string getSessionToken(string userId) {
            // encrompt user id
            return AesCrypt.encrypt(userId, cryptKey);
        }

        public virtual string getUserId(string sessionToken) {
            // decrompt user id
            return AesCrypt.decrypt(sessionToken, cryptKey);
        }
    }
}