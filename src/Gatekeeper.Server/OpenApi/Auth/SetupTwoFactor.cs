using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Responses;

namespace Gatekeeper.Server.OpenApi.Auth {
    public class SetupTwoFactor : RouteMetaData {
        public override string Description => "Set up 2FA on a user account";
        public override string Tag => GateApiConstants.Tags.USER_MANAGEMENT;
        public override string SecuritySchema => GateApiConstants.Security.USER_BEARER_AUTH;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Unauthorized,
                Description = $"Provided credentials were not accepted",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"A {nameof(TotpSetupResponse)} object",
                Response = typeof(TotpSetupResponse)
            }
        };
    }
}