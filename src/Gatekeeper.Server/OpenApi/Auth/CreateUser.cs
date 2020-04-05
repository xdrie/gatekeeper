using System.Collections.Generic;
using System.Net;
using Carter.OpenApi;
using Gatekeeper.Server.Models.Requests;
using Gatekeeper.Server.Models.Responses;

namespace Gatekeeper.Server.OpenApi.Auth {
    public class CreateUser : RouteMetaData {
        public override string Description => "Register a new user account";
        public override string Tag => GateApiConstants.Tags.USER_MANAGEMENT;

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
                Description = $"A {nameof(RegisterRequest)} for a new user",
                Request = typeof(RegisterRequest)
            }
        };
    }
}