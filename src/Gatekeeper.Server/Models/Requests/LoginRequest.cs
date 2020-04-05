using FluentValidation;

namespace Gatekeeper.Server.Models.Requests {
    public class LoginRequest {
        public string username { get; set; }
        public string password { get; set; }

        public class Validator : AbstractValidator<LoginRequest> {
            public Validator() {
                RuleFor(x => x.username).NotEmpty();
                RuleFor(x => x.password).NotEmpty();
            }
        }
    }
}