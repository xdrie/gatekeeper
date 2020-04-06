using System.Linq;
using FrenchFry.Demo.Config;
using FrenchFry.Demo.Models;
using Gatekeeper.Models.Identity;
using Hexagon.Models;

namespace FrenchFry.Demo.Services {
    public class UserManager : DependencyService<SContext> {
        public UserManager(SContext context) : base(context) { }

        public Account findByUid(string uid) {
            using (var db = serverContext.getDbContext()) {
                return db.accounts.SingleOrDefault(x => x.userId == uid);
            }
        }

        public void register(PublicUser remoteUser) {
            var account = new Account(remoteUser.uuid);
            using (var db = serverContext.getDbContext()) {
                db.accounts.Add(account);
                db.SaveChanges();
            }
        }

        public void save(Account account) {
            using (var db = serverContext.getDbContext()) {
                db.accounts.Update(account);
                db.SaveChanges();
            }
        }
    }
}