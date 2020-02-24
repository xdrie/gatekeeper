using FluentValidation;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Models.Requests {
    public class UserCreateRequest {
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string pronouns { get; set; }
        public string isRobot { get; set; }
        
        public class Validator : AbstractValidator<UserCreateRequest> {
            public const string NOT_ROBOT_PROMISE = "I am not a robot";

            public Validator() {
                RuleFor(x => x.username).NotEmpty().Length(4, 32);
                RuleFor(x => x.name).NotEmpty().MaximumLength(32);
                RuleFor(x => x.email).NotEmpty().EmailAddress();
                RuleFor(x => x.password).NotEmpty().MinimumLength(8);
                RuleFor(x => x.isRobot).Equal(NOT_ROBOT_PROMISE);
                RuleFor(x => x.pronouns).IsEnumName(typeof(User.Pronouns), false);
            }
        }
    }
}