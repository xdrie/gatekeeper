using System;
using System.Net;
using Carter.ModelBinding;
using Degate.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.Remote;
using Hexagon;
using Hexagon.Modules;
using Hexagon.Serialization;
using Hexagon.Utilities;

namespace Degate.Modules {
    public abstract class GateLinkModule<TContext> : ApiModule<TContext>
        where TContext : ServerContext, IDegateContext {
        protected GateLinkModule(string path, TContext serverContext) : base(path, serverContext) {
            Post("/", async (req, res) => {
                // token is in form data
                var gateReq = await req.Bind<Token>();

                // - import the token

                if (!serverContext.gateAuthClient.validate(gateReq)) {
                    res.StatusCode = (int) HttpStatusCode.UnprocessableEntity; // invalid
                    return;
                }

                // now, import the identity
                var identity = await serverContext.gateAuthClient.getRemoteIdentity(gateReq);
                if (identity == null) {
                    res.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return;
                }

                // - valid identity!
                res.StatusCode = (int) HttpStatusCode.Accepted;

                // compute the session id
                var sessionId = Convert.ToBase64String(Hasher.sha256(identity.user.uuid));
                if (!serverContext.sessions.exists(sessionId)) {
                    // store identity in a session
                    var sess = serverContext.sessions.create(sessionId);
                    sess.jar.Register<RemoteAuthentication>(identity);
                }

                // display the user's info
                await res.respondSerialized(new GateUser(identity, sessionId));
            });
        }
    }
}