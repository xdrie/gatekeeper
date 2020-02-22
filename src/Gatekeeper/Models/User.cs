namespace Gatekeeper.Models {
    public class User {
        public string Username { get; set; }
        public string Name { get; set; }
    }

    public enum Pronoun {
        TheyThem,
        HeHim,
        SheHer
    }
}