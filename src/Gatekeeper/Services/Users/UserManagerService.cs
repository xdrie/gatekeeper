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

        public (User, Token) registerUser(UserCreateRequest request) {
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
                password = cryptPassword,
                pronouns = request.pronouns,
                verification = StringUtils.secureRandomString(8),
                registered = DateTime.Now
            };

            if (!serverContext.config.server.production) { // if in development, set a default verification code
                user.verification = DevelopmentConstants.DEFAULT_VERIFICATION;
            }
            
            // create an access token
            var tokenSource = new AccessTokenSource(serverContext);
            var token = tokenSource.issueRoot(user);

            using (var db = new AppDbContextFactory().create()) {
                db.users.Add(user); // add user
                db.tokens.Add(token); // add token
                db.SaveChanges();
            }

            // var userMetricsService = new UserMetricsService(serverContext);
            // var metrics = userMetricsService.create(user);
            // userMetricsService.log(user.identifier, MetricsEventType.Auth);

            return (user, token);
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