using System.Linq;
using FrenchFry.Demo.Config;
using FrenchFry.Demo.Models;
using Hexagon.Models;

namespace FrenchFry.Demo.Services {
    public class UserManager : DependencyService<SContext> {
        public UserManager(SContext context) : base(context) { }

        public User findByUid(string uid) {
            using (var db = serverContext.getDbContext()) {
                return db.users.SingleOrDefault(x => x.userId == uid);
            }
        }
    }
}