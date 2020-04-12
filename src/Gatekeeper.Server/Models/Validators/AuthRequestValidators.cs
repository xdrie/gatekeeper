using FluentValidation;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;

namespace Gatekeeper.Server.Models.Validators {
    public static class AuthRequestValidators {
        public class RegisterRequestValidator : AbstractValidator<RegisterRequest> {
            public const string NOT_ROBOT_PROMISE = "I am not a robot";

            public RegisterRequestValidator() {
                RuleFor(x => x.username).NotEmpty().Length(4, 32);
                RuleFor(x => x.name).NotEmpty().MaximumLength(32);
                RuleFor(x => x.email).NotEmpty().EmailAddress();
                RuleFor(x => x.password).NotEmpty().MinimumLength(8);
                RuleFor(x => x.isRobot).Equal(NOT_ROBOT_PROMISE);
                RuleFor(x => x.pronouns).NotEmpty().IsEnumName(typeof(User.Pronouns), false);
            }
        }

        public class LoginRequestValidator : AbstractValidator<LoginRequest> {
            public LoginRequestValidator() {
                RuleFor(x => x.username).NotEmpty();
                RuleFor(x => x.password).NotEmpty();
            }
        }

        public class LoginRequestTwoFactorValidator : AbstractValidator<LoginRequestTwoFactor> {
            public LoginRequestTwoFactorValidator() {
                RuleFor(x => x.username).NotEmpty();
                RuleFor(x => x.password).NotEmpty();
                RuleFor(x => x.otpcode).NotEmpty();
            }
        }

        public class TwoFactorConfirmRequestValidator : AbstractValidator<TwoFactorConfirmRequest> {
            public TwoFactorConfirmRequestValidator() {
                RuleFor(x => x.otpcode).NotEmpty();
            }
        }

        public class UpdateGroupRequestValidator : AbstractValidator<UpdateGroupRequest> {
            public UpdateGroupRequestValidator() {
                RuleFor(x => x.userUuid).NotEmpty();
                RuleFor(x => x.type).IsEnumName(typeof(Group.UpdateType), false);
                RuleFor(x => x.groups).NotEmpty();
            }
        }
    }
}