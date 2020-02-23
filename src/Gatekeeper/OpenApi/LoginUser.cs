using System.Collections.Generic;
using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;

namespace Gatekeeper.OpenApi {
    public class LoginUser : RouteMetaData {
        public override string Description { get; } = "Login to a user account";
        public override string Tag { get; } = OpenApiTags.USER_MANAGEMENT;

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
                Description = $"The corresponding {nameof(AuthenticatedUser)} object",
                Response = typeof(AuthenticatedUser)
            }
        };

        public override RouteMetaDataRequest[] Requests { get; } = {
            new RouteMetaDataRequest {
                Description = $"A {nameof(UserLoginRequest)} for a new user",
                Request = typeof(UserLoginRequest)
            }
        };
    }
}