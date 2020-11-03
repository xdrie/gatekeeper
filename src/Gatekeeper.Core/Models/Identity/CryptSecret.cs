#region

#endregion

namespace Gatekeeper.Models.Identity {
    public class HashedSecret : DatabaseObject {
        public string hash { get; set; }

        public static HashedSecret withDefaultParameters() {
            return new HashedSecret { };
        }
    }
}