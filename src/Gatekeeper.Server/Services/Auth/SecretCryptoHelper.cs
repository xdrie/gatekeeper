#region

using System.Security.Cryptography;
using System.Text;
using Gatekeeper.Models.Identity;

#endregion

namespace Gatekeeper.Server.Services.Auth {
    public class SecretCryptoHelper {
        private CryptSecret cryptSecret { get; }

        public SecretCryptoHelper(CryptSecret cryptSecret) {
            this.cryptSecret = cryptSecret;
        }

        private byte[] generateSalt() {
            var len = cryptSecret.saltLength;
            var bytes = new byte[len];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(bytes);
            }

            return bytes;
        }

        private byte[] calculateSecretHash(byte[] password, byte[] salt) {
            var iter = cryptSecret.iterations;
            var len = cryptSecret.length;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iter)) {
                return deriveBytes.GetBytes(len);
            }
        }

        public void storeSecret(string secret) {
            cryptSecret.salt = generateSalt();
            cryptSecret.hash = calculateSecretHash(Encoding.UTF8.GetBytes(secret), cryptSecret.salt);
        }

        public byte[] hashCleartext(string secret) {
            var hash = calculateSecretHash(Encoding.UTF8.GetBytes(secret), cryptSecret.salt);
            return hash;
        }
    }
}