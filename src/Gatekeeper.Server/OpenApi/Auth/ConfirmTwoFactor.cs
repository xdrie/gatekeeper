using System.Collections.Generic;
using System.Net;
using Carter.OpenApi;

namespace Gatekeeper.Server.OpenApi.Auth {
    public class ConfirmTwoFactor : RouteMetaData {
        public override string Description => "Confirm 2FA setup on user account";
        public override string Tag => GateApiConstants.Tags.USER_MANAGEMENT;
        public override string SecuritySchema => GateApiConstants.Security.USER_BEARER_AUTH;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.UnprocessableEntity,
                Description = "A list of issues with the request",
                Response = typeof(IEnumerable<dynamic>)
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Unauthorized,
                Description = $"Provided credentials were not accepted",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = "2FA setup successfully confirmed"
            }
        };
    }
}