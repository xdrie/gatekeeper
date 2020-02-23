using FluentValidation;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Models.Requests {
    public class UserCreateRequest {
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public User.Pronouns pronouns { get; set; }
        public string isRobot { get; set; }
    }

    public class UserRegistrationValidator : AbstractValidator<UserCreateRequest> {
        public UserRegistrationValidator() {
            RuleFor(x => x.username).NotEmpty().Length(4, 32);
            RuleFor(x => x.name).NotEmpty().MaximumLength(32);
            RuleFor(x => x.email).NotEmpty().EmailAddress();
            RuleFor(x => x.password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.isRobot).Equal("I am not a robot");
        }
    }
}