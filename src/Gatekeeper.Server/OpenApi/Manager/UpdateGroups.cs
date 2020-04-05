using System.Net;
using Carter.OpenApi;
using Gatekeeper.Server.Models.Identity;
using Gatekeeper.Server.Models.Requests;

namespace Gatekeeper.Server.OpenApi.Manager {
    public class UpdateGroups : RouteMetaData {
        public override string Description => "Update groups for a user";
        public override string Tag => GateApiConstants.Tags.ADMIN;
        public override string SecuritySchema => GateApiConstants.Security.USER_BEARER_AUTH;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Unauthorized,
                Description = $"Provided authorization was not accepted",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"The request was successfully processed",
            }
        };

        public override RouteMetaDataRequest[] Requests { get; } = {
            new RouteMetaDataRequest {
                Description = $"An {nameof(UpdateGroupRequest)} with the requested changes",
                Request = typeof(UpdateGroupRequest)
            }
        };
    }
}