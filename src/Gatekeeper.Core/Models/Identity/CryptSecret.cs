#region

#endregion

namespace Gatekeeper.Models.Identity {
    public class CryptSecret : DatabaseObject {
        public string hash { get; set; }

        public static CryptSecret withDefaultParameters() {
            return new CryptSecret { };
        }
    }
}