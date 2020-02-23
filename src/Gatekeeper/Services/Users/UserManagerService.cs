using System;
using System.Linq;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Requests;
using Gatekeeper.Services.Database;

namespace Gatekeeper.Services.Users {
    public class UserManagerService : DependencyObject {
        public UserManagerService(SContext context) : base(context) { }

        public int registeredUserCount => throw new NotImplementedException();

        public User registerUser(UserCreateRequest createReqData) {
            throw new NotImplementedException();
        }

        private User loadPassword(User regUser) {
            using (var db = new AppDbContextFactory().create()) {
                var user = db.users.First(x => x.id == regUser.id);
                db.Entry(user).Reference(x => x.password).Load();
                return user;
            }
        }

        public User findByUser(string username) {
            using (var db = new AppDbContextFactory().create()) {
                return db.users.FirstOrDefault(x => x.username == username);
            }
        }
    }
}