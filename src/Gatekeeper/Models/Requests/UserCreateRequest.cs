using FluentValidation;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Models.Requests {
    public class UserCreateRequest {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Pronoun Pronouns { get; set; }
        public string IsRobot { get; set; }
    }

    public class UserRegistrationValidator : AbstractValidator<UserCreateRequest> {
        public UserRegistrationValidator() {
            RuleFor(x => x.Username).NotEmpty().Length(4, 32);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(32);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.IsRobot).Equal("I am not a robot");
        }
    }
}