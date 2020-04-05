using FluentValidation;

namespace Gatekeeper.Server.Models.Requests {
    public class LoginRequestTwoFactor : LoginRequest {
        public string otpcode { get; set; }

        public new class Validator : AbstractValidator<LoginRequestTwoFactor> {
            public Validator() {
                RuleFor(x => x.username).NotEmpty();
                RuleFor(x => x.password).NotEmpty();
                RuleFor(x => x.otpcode).NotEmpty();
            }
        }
    }
}