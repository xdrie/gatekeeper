#region

using System.Security.Cryptography;
using System.Text;
using Gatekeeper.Models.Identity;

#endregion

namespace Gatekeeper.Services.Auth {
    public class SecretCryptoHelper {
        private CryptSecret secret { get; }

        public SecretCryptoHelper(CryptSecret secret) {
            this.secret = secret;
        }

        public byte[] generateSalt() {
            var len = secret.saltLength;
            var bytes = new byte[len];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(bytes);
            }

            return bytes;
        }

        private byte[] calculateSecretHash(byte[] password, byte[] salt) {
            var iter = secret.iterations;
            var len = secret.length;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iter)) {
                return deriveBytes.GetBytes(len);
            }
        }

        public byte[] calculateSecretHash(string password, byte[] salt) {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return calculateSecretHash(passwordBytes, salt);
        }
    }
}