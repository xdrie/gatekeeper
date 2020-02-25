using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Gatekeeper.Tests.Utilities {
    public static class HttpClientExtensions {
        public const string MIME_JSON = "application/json";

        public static Task<HttpResponseMessage> createAsJsonRequest(
            Func<string, HttpContent, Task<HttpResponseMessage>> makeRequest, string requestUri, object obj) {
            return makeRequest(requestUri,
                new ObjectContent(obj.GetType(), obj, new JsonMediaTypeFormatter(), MIME_JSON));
        }

        public static Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient client, string requestUri, object obj) {
            return createAsJsonRequest(client.PatchAsync, requestUri, obj);
        }
    }
}