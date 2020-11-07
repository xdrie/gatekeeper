#region

using Gatekeeper.Models.Identity;
using Isopoh.Cryptography.Argon2;

#endregion

namespace Gatekeeper.Server.Services.Auth {
    public class PasswordHasher {
        private HashedSecret hashedSecret { get; }

        public PasswordHasher(HashedSecret hashedSecret) {
            this.hashedSecret = hashedSecret;
        }

        /// <summary>
        /// hash a password using the default algorithm
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string hash(string password) {
            return Argon2.Hash(password);
        }
        
        /// <summary>
        /// hash a password and store it in the HashedSecret object
        /// </summary>
        /// <param name="secret"></param>
        public void store(string secret) {
            hashedSecret.hash = hash(secret);
        }

        /// <summary>
        /// compare a specified password with an existing stored hash
        /// </summary>
        /// <param name="testPassword"></param>
        /// <returns></returns>
        public bool verify(string testPassword) {
            return Argon2.Verify(hashedSecret.hash, testPassword);
        }
    }
}