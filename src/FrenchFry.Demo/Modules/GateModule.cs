using System;
using System.Net;
using Carter.ModelBinding;
using FrenchFry.Demo.Config;
using Gatekeeper.Models.Identity;
using Hexagon.Modules;
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
                }
                
                // now, import the identity
                res.StatusCode = (int) HttpStatusCode.Accepted;
            });
        }
    }
}