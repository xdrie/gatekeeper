using System;
using System.Threading.Tasks;
using FluentValidation;
using Gatekeeper.Server.Config;
using Hexagon.Serialization;
using Hexagon.Web;
using Microsoft.AspNetCore.Http;

namespace Gatekeeper.Server.Modules.Test {
    public class BeanModule : GateApiModule {
        class BeanRequest {
            public string name { get; set; }

            class Validator : AbstractValidator<BeanRequest> {
                public Validator() {
                    RuleFor(x => x.name).NotEmpty();
                }
            }
        }

        class BeanResponse {
            public string name { get; set; }
            public bool delicious { get; set; }
        }

        public BeanModule(SContext serverContext) : base("/bean", serverContext) {
            createGet<BeanRequest, BeanResponse>("/order1", async (req) => {
                return new BeanResponse {
                    name = req.name,
                    delicious = true
                };
            });
        }

        public async Task validateThenProcess<TRequest, TResponse>(HttpRequest req, HttpResponse res,
            Func<TRequest, Task<TResponse>> process) where TRequest : class {
            var validatedReq = await this.validateRequest<TRequest>(req, res);
            if (!validatedReq.isValid) return;
            var reqModel = validatedReq.request;
            var response = await process(reqModel);
            await res.respondSerialized(response);
        }

        public void createGet<TRequest, TResponse>(string route, Func<TRequest, Task<TResponse>> process)
            where TRequest : class {
            Get(route, async (req, res) => {
                // call validator first, then process
                await validateThenProcess(req, res, process);
            });
        }
    }
}