using System;
using System.Collections;
using System.Linq;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Services.Auth;
using Hexagon.Utilities;

namespace Gatekeeper.Services.Users {
    public class UserManagerService : DependencyObject {
        public UserManagerService(SContext context) : base(context) { }

        public User registerUser(CreateUserRequest request) {
            if (findByUsername(request.username) != null)
                throw new UserAlreadyExistsException("a user with the same username already exists");
            // encrypt the password
            var cryptPassword = CryptSecret.withDefaultParameters();
            var cryptoHelper = new SecretCryptoHelper(cryptPassword);
            cryptoHelper.storeSecret(request.password);
            // create the user
            var user = new User {
                name = request.name,
                username = request.username,
                email = request.email,
                uuid = Guid.NewGuid().ToString("N"),
                password = cryptPassword,
                pronouns = (User.Pronouns) Enum.Parse(typeof(User.Pronouns), request.pronouns),
                verification = StringUtils.secureRandomString(8),
                registered = DateTime.Now
            };

#if DEBUG
            if (serverContext.config.server.development) { // if in development, set a default verification code
                user.verification = DevelopmentConstants.DEFAULT_VERIFICATION;
            }
#endif

            using (var db = serverContext.getDbContext()) {
                db.users.Add(user); // add user
                db.SaveChanges();
            }

            return user;
        }

        public Token issueRootToken(int userId) {
            // create an access token
            var token = serverContext.tokenAuthenticator.issueRoot();

            using (var db = serverContext.getDbContext()) {
                token.user = db.users.Find(userId);
                db.tokens.Add(token); // add token
                db.SaveChanges();
            }

            return token;
        }

        public void deleteUser(int userId) {
            using (var db = serverContext.getDbContext()) {
                // delete all associated tokens
                var userTokens = db.tokens.Where(x => x.user.dbid == userId);
                db.tokens.RemoveRange(userTokens);
                // delete user
                var user = db.users.Find(userId);
                db.users.Remove(user);
                db.SaveChanges();
            }
        }

        public void updateUser(User user) {
            using (var db = serverContext.getDbContext()) {
                db.users.Update(user);
                db.SaveChanges();
            }
        }

        public bool checkPassword(string password, User user) {
            user = loadPassword(user);
            var ret = false;
            // calculate hash and compare
            var cryptoHelper = new SecretCryptoHelper(user.password);
            var hashedPassword = cryptoHelper.hashCleartext(password);
            ret = StructuralComparisons.StructuralEqualityComparer.Equals(hashedPassword, user.password.hash);
            return ret;
        }

        public void setupTotpLock(User user, Token currentToken) {
            user.totpEnabled = true; // enable otp field
            updateUser(user);
            // revoke all other tokens
            using (var db = serverContext.getDbContext()) {
                var otherTokens = db.tokens.Where(x => x.content != currentToken.content && x.user.dbid == user.dbid);
                db.tokens.RemoveRange(otherTokens);
                db.SaveChanges();
            }
        }

        public User? findByUsername(string username) {
            using (var db = serverContext.getDbContext()) {
                return db.users.FirstOrDefault(x => x.username == username);
            }
        }

        private User loadPassword(User forUser) {
            using (var db = serverContext.getDbContext()) {
                var user = db.users.First(x => x.dbid == forUser.dbid);
                db.Entry(user).Reference(x => x.password).Load();
                return user;
            }
        }

        public class UserAlreadyExistsException : ApplicationException {
            public UserAlreadyExistsException(string message) : base(message) { }
        }
    }
}