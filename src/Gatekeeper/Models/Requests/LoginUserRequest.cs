using FluentValidation;

namespace Gatekeeper.Models.Requests {
    public class LoginUserRequest {
        public string username { get; set; }
        public string password { get; set; }

        public class Validator : AbstractValidator<LoginUserRequest> {
            public Validator() {
                RuleFor(x => x.username).NotEmpty();
                RuleFor(x => x.password).NotEmpty();
            }
        }
    }
}