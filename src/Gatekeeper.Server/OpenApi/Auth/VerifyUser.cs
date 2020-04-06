using System.Net;
using Carter.OpenApi;

namespace Gatekeeper.Server.OpenApi.Auth {
    public class VerifyUser : RouteMetaData {
        public override string Description => "Confirm new user verification code";
        public override string Tag => GateApiConstants.Tags.USER_MANAGEMENT;
        public override string SecuritySchema => GateApiConstants.Security.USER_BEARER_AUTH;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Unauthorized,
                Description = $"Provided credentials were not accepted",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = "User account successfully verified"
            }
        };
    }
}