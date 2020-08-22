using System;
using Gatekeeper.Models.Identity;
using Hexagon;
using Hexagon.Models;
using Iri.Glass.Utilities;

namespace Degate.Services {
    /// <summary>
    /// manage resolution of remote authentication sessions
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class AuthSessionResolver<TContext> : DependencyService<TContext>, IAuthSessionResolver
        where TContext : ServerContext {
        public string cryptKey { get; }
        public TimeSpan sessionValidity { get; set; } = TimeSpan.FromDays(1);

        public AuthSessionResolver(TContext context) : base(context) {
            cryptKey = Convert.ToBase64String(AesCrypt.randomBytes(AesCrypt.KEY_LENGTH));
        }

        public virtual string issueSession(RemoteAuthentication identity) {
            // get a session token
            var sessionId = getSessionToken(identity.user.uuid);

            // store identity in a session
            var sess = serverContext.sessions.create(sessionId, sessionValidity);
            sess.jar.Register<RemoteAuthentication>(identity);

            return sessionId;
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