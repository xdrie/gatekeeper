using System;
using System.Collections;
using System.Linq;
using Gatekeeper.Models.Access;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.Services.Auth;
using Hexagon.Models;
using Iri.Glass.Utilities;

namespace Gatekeeper.Server.Services.Users {
    public class UserManagerService : DependencyService<SContext> {
        public UserManagerService(SContext context) : base(context) { }

        public User registerUser(RegisterRequest request) {
            if (findByUsername(request.username) != null)
                throw new UserAlreadyExistsException("a user with the same username already exists");
            if (findByEmail(request.email) != null)
                throw new UserAlreadyExistsException("a user with the same email already exists");
            // encrypt the password
            var cryptPassword = HashedSecret.withDefaultParameters();
            var cryptoHelper = new PasswordHasher(cryptPassword);
            cryptoHelper.store(request.password);
            // create the user
            var user = new User {
                name = request.name,
                username = request.username,
                email = request.email,
                uuid = Guid.NewGuid().ToString("N"),
                password = cryptPassword,
                pronouns = Enum.Parse<User.Pronouns>(request.pronouns, true),
                verification = StringUtils.secureRandomString(8),
                registered = DateTime.UtcNow,
                groups = new string[0]
            };
            // - set default settings
            // add groups from default groups
            user.groups = serverContext.config.users.defaultGroups.ToArray();

#if DEBUG
            if (serverContext.config.server.development) {
                // if in development, set a default verification code
                user.verification = Constants.DEFAULT_VERIFICATION;
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
            var token = serverContext.tokenResolver.issueRoot();

            return issueTokenFor(userId, token);
        }

        public Token issueTokenFor(int userId, Token token) {
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
                var userTokens = db.tokens.Where(x => x.user.id == userId);
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

        public void updateGroupMembership(int userId, string groupName, Group.UpdateType updateType) {
            using (var db = serverContext.getDbContext()) {
                var user = db.users.SingleOrDefault(x => x.id == userId);
                switch (updateType) {
                    case Group.UpdateType.Add:
                        // make sure groups aren't duplicated
                        if (user.groups.Contains(groupName)) break;
                        user.groups = user.groups.Concat(new[] {groupName}).ToArray();

                        break;
                    case Group.UpdateType.Remove:
                        user.groups = user.groups.Except(new[] {groupName}).ToArray();

                        break;
                }

                db.SaveChanges();
            }
        }

        public bool checkPassword(string password, User user) {
            user = loadPassword(user);
            var ret = false;
            // calculate hash and compare
            var cryptoHelper = new PasswordHasher(user.password);
            return cryptoHelper.verify(password);
        }

        public void setupTotpLock(User user, Token currentToken) {
            user.totpEnabled = true; // enable otp field
            updateUser(user);
            // revoke all other tokens
            using (var db = serverContext.getDbContext()) {
                var otherTokens = db.tokens.Where(x => x.content != currentToken.content && x.user.id == user.id);
                db.tokens.RemoveRange(otherTokens);
                db.SaveChanges();
            }
        }

        public User? findByUsername(string username) {
            using (var db = serverContext.getDbContext()) {
                return db.users.SingleOrDefault(x => x.username == username);
            }
        }

        public User? findByEmail(string email) {
            using (var db = serverContext.getDbContext()) {
                return db.users.SingleOrDefault(x => x.email == email);
            }
        }

        public User? findByUuid(string uuid) {
            using (var db = serverContext.getDbContext()) {
                return db.users.SingleOrDefault(x => x.uuid == uuid);
            }
        }

        private User loadPassword(User forUser) {
            using (var db = serverContext.getDbContext()) {
                var user = db.users.First(x => x.id == forUser.id);
                db.Entry(user).Reference(x => x.password).Load();
                return user;
            }
        }

        public User loadGroups(User forUser) {
            using (var db = serverContext.getDbContext()) {
                var user = db.users.First(x => x.id == forUser.id);
                // disable, because we are currently using array-backed serialization
                // db.Entry(user).Collection(x => x.groups).Load();
                return user;
            }
        }

        public class UserAlreadyExistsException : ApplicationException {
            public UserAlreadyExistsException(string message) : base(message) { }
        }
    }
}