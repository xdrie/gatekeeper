using System;
using System.Net;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Remote {
    public class GateAuthClient {
        public string app { get; }
        public string secret { get; }

        public GateAuthClient(string app, string secret) {
            this.app = app;
            this.secret = secret;
        }

        public bool validate(Token token) {
            if (string.IsNullOrEmpty(token.content)) return false; // ensure content exists
            if (DateTime.UtcNow >= token.expires) return false; // ensure date validity

            return true;
        }
    }
}