using FrenchFry.Demo.Config;
using Hexagon.Models;

namespace FrenchFry.Demo.Services {
    public class UserManager : DependencyService<SContext> {
        public UserManager(SContext context) : base(context) { }
    }
}