using FluentValidation;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Hexagon.Utilities;

namespace Gatekeeper.Server.Models.Validators {
    public static class AuthRequestValidators {
        public class RegisterRequestValidator : AbstractValidator<RegisterRequest> {
            public const string NOT_ROBOT_PROMISE = "I am not a robot";

            public RegisterRequestValidator() {
                RuleFor(x => x.username).NotEmpty().Length(4, 32).Matches(StringValidator.identifierRegex);
                RuleFor(x => x.name).NotEmpty().MaximumLength(32);
                RuleFor(x => x.email).NotEmpty().EmailAddress();
                RuleFor(x => x.password).NotEmpty().MinimumLength(8);
                RuleFor(x => x.isRobot).Equal(NOT_ROBOT_PROMISE);
                RuleFor(x => x.pronouns).IsEnumName(typeof(User.Pronouns), false);
            }
        }
    }
}