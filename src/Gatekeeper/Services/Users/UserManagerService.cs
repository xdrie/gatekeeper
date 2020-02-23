using System;
using System.Collections;
using System.Linq;
using System.Security;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Services.Auth;
using Gatekeeper.Services.Database;
using Hexagon.Utilities;

namespace Gatekeeper.Services.Users {
    public class UserManagerService : DependencyObject {
        public UserManagerService(SContext context) : base(context) { }

        public int registeredUserCount => throw new NotImplementedException();

        public User registerUser(UserCreateRequest request) {
            if (findByUsername(request.username) != null)
                throw new UserAlreadyExistsException("a user with the same username already exists");
            // encrypt the password
            var password = CryptSecret.withDefaultParameters();
            var cryptoHelper = new SecretCryptoHelper(password);
            var pwSalt = cryptoHelper.generateSalt();
            var encryptedPassword = cryptoHelper.calculateSecretHash(request.password, pwSalt);
            // create the user
            password.salt = pwSalt;
            password.secret = encryptedPassword;
            var user = new User {
                name = request.name,
                username = request.username,
                email = request.email,
                password = password,
                pronouns = request.pronouns,
                verification = StringUtils.secureRandomString(8),
                registered = DateTime.Now
            };

            if (!serverContext.config.server.production) {
                user.verification = DevelopmentConstants.DEFAULT_VERIFICATION;
            }

            using (var db = new AppDbContextFactory().create()) {
                // add user to database
                db.users.Add(user);
                db.SaveChanges();
            }

            // var userMetricsService = new UserMetricsService(serverContext);
            // var metrics = userMetricsService.create(user);
            // userMetricsService.log(user.identifier, MetricsEventType.Auth);

            return user;
        }
        
        public bool checkPassword(string password, User user) {
            user = loadPassword(user);
            var ret = false;
            // calculate hash and compare
            var cryptoHelper = new SecretCryptoHelper(user.password);
            var pwKey =
                cryptoHelper.calculateSecretHash(password, user.password.salt);
            ret = StructuralComparisons.StructuralEqualityComparer.Equals(pwKey, user.password.secret);
            return ret;
        }

        public User? findByUsername(string username) {
            using (var db = new AppDbContextFactory().create()) {
                return db.users.FirstOrDefault(x => x.username == username);
            }
        }
        
        private User loadPassword(User forUser) {
            using (var db = new AppDbContextFactory().create()) {
                var user = db.users.First(x => x.id == forUser.id);
                db.Entry(user).Reference(x => x.password).Load();
                return user;
            }
        }

        public class UserAlreadyExistsException : ApplicationException {
            public UserAlreadyExistsException(string message) : base(message) { }
        }
    }
}