using System.Collections.Generic;
using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Access;

namespace Gatekeeper.OpenApi.Users {
    public class GetMyGroups : RouteMetaData {
        public override string Description => "Get current user group membership";
        public override string Tag => GateApiConstants.Tags.USER_DIRECTORY;
        public override string SecuritySchema => GateApiConstants.Security.USER_BEARER_AUTH;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Unauthorized,
                Description = $"Provided authorization was not accepted",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"A {nameof(IEnumerable<Group>)} containing the users groups",
                Response = typeof(IEnumerable<Group>)
            }
        };
    }
}