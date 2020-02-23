using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.OpenApi.Users {
    public class GetMyself : RouteMetaData {
        public override string Description => "Get current user metadata";
        public override string Tag => OpenApiConstants.Tags.USER_DIRECTORY;
        public override string SecuritySchema => OpenApiConstants.Security.USER_TOKEN;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Unauthorized,
                Description = $"Provided authorization was not accepted",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"A {nameof(AuthenticatedUser)} representing the current user",
                Response = typeof(AuthenticatedUser)
            }
        };
    }
}