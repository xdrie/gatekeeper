using FluentValidation;
using Gatekeeper.Models.Access;

namespace Gatekeeper.Models.Requests {
    public class UpdateGroupRequest {
        public string userUuid { get; set; }
        public string type { get; set; }
        public string[] groups { get; set; }

        public class Validator : AbstractValidator<UpdateGroupRequest> {
            public Validator() {
                RuleFor(x => x.userUuid).NotEmpty();
                RuleFor(x => x.type).IsEnumName(typeof(Group.UpdateType), false);
                RuleFor(x => x.groups).NotEmpty();
            }
        }
    }
}