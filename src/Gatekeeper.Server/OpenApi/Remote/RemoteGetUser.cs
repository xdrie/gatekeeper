using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Server.OpenApi.Remote {
    public class RemoteGetUser : RouteMetaData {
        public override string Description => "Get remote user information";
        public override string Tag => GateApiConstants.Tags.REMOTE_APP;
        public override string SecuritySchema => GateApiConstants.Security.REMOTE_APP_APIKEY;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.Unauthorized,
                Description = $"Provided authorization was not accepted",
            },
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"A {nameof(PublicUser)} representing the current user",
                Response = typeof(PublicUser)
            }
        };
    }
}