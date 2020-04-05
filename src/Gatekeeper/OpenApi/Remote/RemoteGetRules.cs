using System.Collections.Generic;
using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Access;

namespace Gatekeeper.OpenApi.Remote {
    public class RemoteGetRules : RouteMetaData {
        public override string Description => "Remotely get app rules";
        public override string Tag => GateApiConstants.Tags.REMOTE_APP;
        public override string SecuritySchema => GateApiConstants.Security.REMOTE_APP_APIKEY;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Unauthorized,
                Description = $"Provided authorization was not accepted",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"A {nameof(IEnumerable<AccessRule>)} with app rules",
                Response = typeof(IEnumerable<AccessRule>)
            }
        };
    }
}