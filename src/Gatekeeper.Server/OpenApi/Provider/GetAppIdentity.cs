using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Server.OpenApi.Provider {
    public class GetAppIdentity : RouteMetaData {
        public override string Description => "Get a token and user information for signing into a given app";
        public override string Tag => GateApiConstants.Tags.AUTH_PROVIDER;
        public override string SecuritySchema => GateApiConstants.Security.USER_BEARER_AUTH;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Unauthorized,
                Description = $"Provided authorization was not accepted",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.NotFound,
                Description = $"Requested resource was not found",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Forbidden,
                Description = $"User does not have access to the app",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"A {nameof(RemoteIdentity)} for the app",
                Response = typeof(RemoteIdentity)
            }
        };
    }
}