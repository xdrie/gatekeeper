#region

using System.Net;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using Carter.Response;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Services.Auth;
using Microsoft.AspNetCore.Http;

#endregion

namespace Gatekeeper.Server.Modules {
    /// <summary>
    /// Defines an API module for the application
    /// </summary>
    public abstract class ApiModule : CarterModule {
        public SContext serverContext { get; }

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