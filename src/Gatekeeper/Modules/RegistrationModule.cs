using System.Net;
using Carter.ModelBinding;
using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Models.Requests;
using Gatekeeper.OpenApi;

namespace Gatekeeper.Modules {
    public class RegistrationModule : ApiModule {
        public RegistrationModule(SContext context) : base("/user", context) {
            Post<CreateUser>("/create", async (req, res) => {
                var createReq = await req.BindAndValidate<UserCreateRequest>();
                if (!createReq.ValidationResult.IsValid) {
                    res.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    await res.Negotiate(createReq.ValidationResult.GetFormattedErrors());
                    return;
                }

                var newUser = new User {
                    Username = createReq.Data.Username,
                    Name = createReq.Data.Name
                };
                await res.Negotiate(newUser);
            });
        }
    }
}