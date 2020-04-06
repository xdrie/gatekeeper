using System;
using System.Net;
using Carter.ModelBinding;
using Carter.Response;
using FrenchFry.Demo.Config;
using Gatekeeper.Models.Identity;
using Hexagon.Modules;
using Hexagon.Serialization;
using Hexagon.Web;

namespace FrenchFry.Demo.Modules {
    public class GateModule : ApiModule<SContext> {
        public GateModule(SContext serverContext) : base("/gate", serverContext) {
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
                res.StatusCode = (int) HttpStatusCode.Accepted;
                // display the user's info
                await res.respondSerialized(identity);
            });
        }
    }
}