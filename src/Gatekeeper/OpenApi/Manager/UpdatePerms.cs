using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;

namespace Gatekeeper.OpenApi.Manager {
    public class UpdatePerms : RouteMetaData {
        public override string Description => "Update permissions for a user";
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
                Description = $"An {nameof(UpdatePermissionRequest)} with the requested changes",
                Request = typeof(UpdatePermissionRequest)
            }
        };
    }
}