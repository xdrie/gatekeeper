using System;
using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Models.Identity;
using Newtonsoft.Json;

namespace Gatekeeper.Remote {
    public class GateAuthClient {
        public string app { get; }
        public Uri server { get; }
        public string secret { get; }

        public GateAuthClient(string app, Uri server, string secret) {
            this.app = app;
            this.server = server;
            this.secret = secret;
        }

        public bool validate(Token token) {
            if (string.IsNullOrEmpty(token.content)) return false; // ensure content exists
            if (DateTime.UtcNow >= token.expires) return false; // ensure date validity

            return true;
        }

        public async Task<RemoteAuthentication> getRemoteIdentity(Token token) {
            var client = new HttpClient();
            // set authentication fields
            client.DefaultRequestHeaders.Add(Constants.APP_SECRET_HEADER, secret);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.content}");
            // request remote auth
            try {
                var resp = await client.GetAsync(new Uri(server, "/a/remote"));
                resp.EnsureSuccessStatusCode();
                // parse response data
                var remoteAuth = JsonConvert.DeserializeObject<RemoteAuthentication>(
                    await resp.Content.ReadAsStringAsync());
                return remoteAuth;
            }
            catch (HttpRequestException ex) {
                // error response. no identity.
                return null;
            }
        }
    }
}