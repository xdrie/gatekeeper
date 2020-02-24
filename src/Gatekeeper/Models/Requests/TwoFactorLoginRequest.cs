using FluentValidation;

namespace Gatekeeper.Models.Requests {
    public class TwoFactorLoginRequest : LoginUserRequest {
        public string otpcode { get; set; }

        public class Validator : AbstractValidator<TwoFactorLoginRequest> {
            public Validator() {
                RuleFor(x => x.username).NotEmpty();
                RuleFor(x => x.password).NotEmpty();
                RuleFor(x => x.otpcode).NotEmpty();
            }
        }
    }
}