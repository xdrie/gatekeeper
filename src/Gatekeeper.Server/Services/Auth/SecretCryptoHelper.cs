#region

using System.Security.Cryptography;
using System.Text;
using Gatekeeper.Models.Identity;
using Isopoh.Cryptography.Argon2;

#endregion

namespace Gatekeeper.Server.Services.Auth {
    public class SecretCryptoHelper {
        private CryptSecret cryptSecret { get; }

        public SecretCryptoHelper(CryptSecret cryptSecret) {
            this.cryptSecret = cryptSecret;
        }

        private string calculateSecretHash(string password) {
            return Argon2.Hash(password);
        }

        public void storeSecret(string secret) {
            cryptSecret.hash = calculateSecretHash(secret);
        }

        public bool verify(string testPassword) {
            return Argon2.Verify(cryptSecret.hash, testPassword);
        }
    }
}