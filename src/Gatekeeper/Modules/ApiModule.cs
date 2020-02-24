#region

using System.Net;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using Carter.Response;
using Gatekeeper.Config;
using Gatekeeper.Services.Auth;
using Microsoft.AspNetCore.Http;

#endregion

namespace Gatekeeper.Modules {
    /// <summary>
    /// Defines an API module for Speercs
    /// </summary>
    public abstract class ApiModule : CarterModule {
        public SContext serverContext { get; }

        public class ValidatedRequest<TRequest> where TRequest : class {
            public TRequest request;
            public bool isValid;

            public ValidatedRequest(TRequest request, bool isValid) {
                this.request = request;
                this.isValid = isValid;
            }

            public static ValidatedRequest<TRequest> failure() => new ValidatedRequest<TRequest>(null, false);
        }

        public async Task<ValidatedRequest<TRequest>> validateRequest<TRequest>(HttpRequest req,
            HttpResponse res) where TRequest : class {
            var reqModel = await req.BindAndValidate<TRequest>();
            if (!reqModel.ValidationResult.IsValid) {
                res.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                await res.Negotiate(reqModel.ValidationResult.GetFormattedErrors());
                return ValidatedRequest<TRequest>.failure();
            }

            return new ValidatedRequest<TRequest>(reqModel.Data, true);
        }

        internal ApiModule(string path, SContext serverContext) : base($"/a{path}") {
            this.serverContext = serverContext;

            Before += context => {
                // look for auth token in request
                var token = default(string);
                var authHeader = context.Request.Headers["Authorization"];
                if (string.IsNullOrWhiteSpace(authHeader)) {
                    return Task.FromResult(true); // continue, but with no auth
                }

                var authHeaderParts = authHeader.ToString().Split(" ");
                if (authHeaderParts.Length != 2) {
                    return Task.FromResult(true);
                }

                var scheme = authHeaderParts[0];
                if (scheme.ToLower() != "bearer") {
                    return Task.FromResult(true);
                }

                token = authHeaderParts[1];

                // call authenticator to resolve access level
                var auther = new ApiAuthenticator(serverContext);
                var identity = auther.resolveIdentity(token);
                context.User = identity;

                return Task.FromResult(true); // continue the pipeline
            };
        }
    }
}