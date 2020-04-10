using System.Net;
using Carter.ModelBinding;
using Degate.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.Remote;
using Hexagon;
using Hexagon.Modules;
using Hexagon.Serialization;

namespace Degate.Modules {
    public abstract class BridgeLinkModule<TContext> : ApiModule<TContext>
        where TContext : ServerContext, IDegateContext {
        protected BridgeLinkModule(string path, TContext serverContext) : base(path, serverContext) {
            Post("/", async (req, res) => {
                // token is in form data
                var gateReq = await req.Bind<Token>();

                // - import the token

                if (!serverContext.remoteAuthClient.validate(gateReq)) {
                    res.StatusCode = (int) HttpStatusCode.UnprocessableEntity; // invalid
                    return;
                }

                // now, import the identity
                var identity = await serverContext.remoteAuthClient.getRemoteIdentity(gateReq);
                if (identity == null) {
                    res.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return;
                }

                // - valid identity! - link the identity to a session
                res.StatusCode = (int) HttpStatusCode.Accepted;
                var sessionId = serverContext.authSessionResolver.issueSession(identity);

                updateRecord(identity);

                // display the user's info
                await res.respondSerialized(new GateUser(identity, sessionId));
            });
        }

        public virtual void updateRecord(RemoteAuthentication remoteAuth) { }
    }
}