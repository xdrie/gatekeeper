using FluentValidation;

namespace Gatekeeper.Models.Requests {
    public class UserLoginRequest {
        public string username { get; set; }
        public string password { get; set; }

        public class Validator : AbstractValidator<UserLoginRequest> {
            public Validator() {
                RuleFor(x => x.username).NotEmpty();
                RuleFor(x => x.password).NotEmpty();
            }
        }
    }
}