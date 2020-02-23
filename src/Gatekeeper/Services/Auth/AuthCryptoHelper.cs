#region

using System.Security.Cryptography;
using System.Text;
using Gatekeeper.Models.Identity;

#endregion

namespace Gatekeeper.Services.Auth {
    public class AuthCryptoHelper {
        private CryptSecret secr { get; }

        public AuthCryptoHelper(CryptSecret secr) {
            this.secr = secr;
        }

        public byte[] generateSalt() {
            var len = secr.saltLength;
            var bytes = new byte[len];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(bytes);
            }

            return bytes;
        }

        private byte[] calculatePasswordHash(byte[] password, byte[] salt) {
            var iter = secr.iterations;
            var len = secr.length;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iter)) {
                return deriveBytes.GetBytes(len);
            }
        }

        public byte[] calculateUserPasswordHash(string password, byte[] salt) {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return calculatePasswordHash(passwordBytes, salt);
        }

        public const int DEFAULT_API_KEY_LENGTH = 42;
    }
}