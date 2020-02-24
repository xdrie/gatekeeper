using FluentValidation;
using Gatekeeper.Models.Access;

namespace Gatekeeper.Models.Requests {
    public class UpdatePermissionRequest {
        public string userUuid { get; set; }
        public string type { get; set; }
        public string[] permissions { get; set; }

        public enum PermissionUpdateType {
            Add,
            Remove
        }

        public class Validator : AbstractValidator<UpdatePermissionRequest> {
            public Validator() {
                RuleFor(x => x.userUuid).NotEmpty();
                RuleFor(x => x.type).IsEnumName(typeof(PermissionUpdateType), false);
                RuleFor(x => x.permissions).NotEmpty();
            }
        }
    }
}