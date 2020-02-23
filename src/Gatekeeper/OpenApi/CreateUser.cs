using System.Collections.Generic;
using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Requests;
using Gatekeeper.Models.Responses;

namespace Gatekeeper.OpenApi {
    public class CreateUser : RouteMetaData {
        public override string Description { get; } = "Register a new user account";
        public override string Tag { get; } = OpenApiTags.USER_MANAGEMENT;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.UnprocessableEntity,
                Description = "A list of issues with the request",
                Response = typeof(IEnumerable<dynamic>)
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Conflict,
                Description = "A user with the same username already exists"
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Created,
                Description = $"A new {nameof(AuthedUserResponse)} object",
                Response = typeof(AuthedUserResponse)
            }
        };

        public override RouteMetaDataRequest[] Requests { get; } = {
            new RouteMetaDataRequest {
                Description = $"A {nameof(UserCreateRequest)} for a new user",
                Request = typeof(UserCreateRequest)
            }
        };
    }
}