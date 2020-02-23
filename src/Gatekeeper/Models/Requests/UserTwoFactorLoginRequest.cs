using FluentValidation;

namespace Gatekeeper.Models.Requests {
    public class UserTwoFactorLoginRequest {
        public string username { get; set; }
        public string password { get; set; }
        public string otpcode { get; set; }

        public class Validator : AbstractValidator<UserTwoFactorLoginRequest> {
            public Validator() {
                RuleFor(x => x.username).NotEmpty();
                RuleFor(x => x.password).NotEmpty();
                RuleFor(x => x.otpcode).NotEmpty();
            }
        }
    }
}