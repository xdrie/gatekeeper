using Hexagon.Models;

namespace FrenchFry.Demo.Models {
    public class Account : ModelObject {
        public string userId { get; set; }
        public long orderedFries { get; set; } = 0;
        
        public Account() {}

        public Account(string userId) {
            this.userId = userId;
        }
    }
}