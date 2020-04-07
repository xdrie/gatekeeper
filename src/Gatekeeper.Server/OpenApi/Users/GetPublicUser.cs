using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Server.OpenApi.Users {
    public class GetPublicUser : RouteMetaData {
        public override string Description => "Get public user information";
        public override string Tag => GateApiConstants.Tags.USER_DIRECTORY;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.NotFound,
                Description = $"Matching user was not found",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"A {nameof(PublicUser)} with user information",
                Response = typeof(PublicUser)
            }
        };

        public override RouteMetaDataRequest[] Requests { get; } = {
            new RouteMetaDataRequest {
                Description = $"A username to find a user by",
            }
        };
    }
}