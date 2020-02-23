using System;
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
                throw new SecurityException("a user with the same username already exists");
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

        private User loadPassword(User regUser) {
            using (var db = new AppDbContextFactory().create()) {
                var user = db.users.First(x => x.id == regUser.id);
                db.Entry(user).Reference(x => x.password).Load();
                return user;
            }
        }

        public User? findByUsername(string username) {
            using (var db = new AppDbContextFactory().create()) {
                return db.users.FirstOrDefault(x => x.username == username);
            }
        }
    }
}