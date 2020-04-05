namespace Gatekeeper.Models.Requests {
    public class RegisterRequest {
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string pronouns { get; set; }
        public string isRobot { get; set; }
    }
}