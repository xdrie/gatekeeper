using System.Collections.Generic;
using System.Net;
using Carter.OpenApi;
using UnifiedAuthService.Models;
using UnifiedAuthService.Models.Requests;

namespace UnifiedAuthService.OpenAPI {
    public class CreateUser : RouteMetaData {
       public override string Description { get; } = "Register a new user account";
       public override string Tag { get; } = "User Management";
       
       public override RouteMetaDataResponse[] Responses { get; } = {
           new RouteMetaDataResponse {
               Code = (int)HttpStatusCode.UnprocessableEntity,
               Description = "A list of issues with the request",
               Response = typeof(IEnumerable<dynamic>)
           },
           new RouteMetaDataResponse {
               Code = (int)HttpStatusCode.Created,
               Description = $"A new {nameof(User)} object",
               Response = typeof(User)
           }
       };

       public override RouteMetaDataRequest[] Requests { get; } = {
           new RouteMetaDataRequest {
               Description = $"A {nameof(UserRegistrationRequest)} for a new user",
               Request = typeof(UserRegistrationRequest)
           }
       };
    }
}