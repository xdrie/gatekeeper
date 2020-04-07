namespace Gatekeeper.Models.Responses {
    public class TotpSetupResponse {
        /// <summary>
        /// base-64 encoded TOTP secret
        /// </summary>
        public string secret { get; set; }
    }
}