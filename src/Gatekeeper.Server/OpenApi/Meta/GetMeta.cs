using System.Net;
using Carter.OpenApi;
using Gatekeeper.Models.Meta;

namespace Gatekeeper.Server.OpenApi.Meta {
    public class GetMeta : RouteMetaData {
        public override string Description => "Get server metadata";
        public override string Tag => GateApiConstants.Tags.SERVER_META;

        public override RouteMetaDataResponse[] Responses { get; } = {
            new RouteMetaDataResponse {
                Code = (int) HttpStatusCode.OK,
                Description = $"A {nameof(ServerMetadata)} object representing server metadata",
                Response = typeof(ServerMetadata)
            }
        };
    }
}