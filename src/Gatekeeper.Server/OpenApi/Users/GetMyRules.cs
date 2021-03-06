using System.Collections.Generic;
using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Access;

namespace Gatekeeper.Server.OpenApi.Users {
    public class GetMyRules : RouteMetaData {
        public override string Description => "Get current user app access rules";
        public override string Tag => GateApiConstants.Tags.USER_DIRECTORY;
        public override string SecuritySchema => GateApiConstants.Security.USER_BEARER_AUTH;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Unauthorized,
                Description = $"Provided authorization was not accepted",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"A {nameof(List<AccessRule>)} containing the user's rules",
                Response = typeof(List<AccessRule>)
            }
        };
    }
}