using FluentValidation;
using Gatekeeper.Server.Config;

namespace Gatekeeper.Server.Modules.Test {
    public class BeanModule : GateApiModule {
        class WenEta {
            public class Request {
                public string thing { get; set; }
            }

            public class Response {
                public string thing { get; set; }
                public string eta { get; set; }
            }

            class Validator : AbstractValidator<Request> {
                public Validator() {
                    RuleFor(x => x.thing).NotEmpty();
                }
            }
        }

        public BeanModule(SContext serverContext) : base("/bean", serverContext) {
            createGet<WenEta.Request, WenEta.Response>("/weneta", async (req) => {
                return new WenEta.Response {
                    thing = req.thing,
                    eta = "son",
                };
            });
            createPost<WenEta.Request, WenEta.Response>("/tweet", async (req) => {
                return new WenEta.Response {
                    thing = req.thing,
                    eta = "ok",
                };
            });
        }
    }
}