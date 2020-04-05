namespace Gatekeeper.Models.Requests {
    public class LoginRequestTwoFactor : LoginRequest {
        public string otpcode { get; set; }
    }
}