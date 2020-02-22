using System.Net;
using Carter;
using Carter.ModelBinding;
using Carter.OpenApi;
using Carter.Response;
using Gatekeeper.Models;
using Gatekeeper.Models.Requests;
using Gatekeeper.OpenAPI;
using Gatekeeper.Services;

namespace Gatekeeper.Modules {
    public class RegisterEndpoint : RouteMetaData {
        
    }
    public class RegistrationModule : CarterModule {
        public RegistrationModule() {
            Post<CreateUser>("/register", async (req, res) => {
                var ureq = await req.BindAndValidate<UserRegistrationRequest>();
                if (!ureq.ValidationResult.IsValid) {
                    res.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    await res.Negotiate(ureq.ValidationResult.GetFormattedErrors());
                    return;
                }

                var newUser = new User {
                    Username = ureq.Data.Username,
                    Name = ureq.Data.Name
                };
                await res.Negotiate(newUser);
            });
        }
    }
}