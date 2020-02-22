namespace Gatekeeper.Models.Identity {
    public class User {
        public string username { get; set; }
        public string name { get; set; }
    }

    public enum Pronoun {
        TheyThem,
        HeHim,
        SheHer
    }
}