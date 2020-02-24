using FluentValidation;

namespace Gatekeeper.Models.Requests {
    public class TwoFactorConfirmRequest {
        public string otpcode { get; set; }
        
        public class Validator : AbstractValidator<TwoFactorConfirmRequest> {
            public Validator() {
                RuleFor(x => x.otpcode).NotEmpty();
            }
        }
    }
}