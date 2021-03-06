using System.Collections.Generic;
using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;

namespace Gatekeeper.Server.OpenApi.Auth {
    public class LoginUser : RouteMetaData {
        public override string Description => "Login to a user account";
        public override string Tag => GateApiConstants.Tags.USER_MANAGEMENT;

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
                Code = (int) HttpStatusCode.FailedDependency,
                Description = $"Two-factor authentication is required",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"The corresponding {nameof(AuthedUserResponse)} object",
                Response = typeof(AuthedUserResponse)
            }
        };

        public override RouteMetaDataRequest[] Requests { get; } = {
            new RouteMetaDataRequest {
                Description = $"A {nameof(LoginRequest)} containing credentials",
                Request = typeof(LoginRequest)
            }
        };
    }
}