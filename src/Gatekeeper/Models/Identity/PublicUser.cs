using System;

namespace Gatekeeper.Models.Identity {
    /// <summary>
    /// Represents user information visible publicly
    /// </summary>
    public class PublicUser {
        public string name { get; set; }
        public string username { get; set; }
        public string? email { get; set; }
        public User.Pronouns pronouns { get; set; }
        public DateTime registered { get; set; }

        public PublicUser() { }

        public PublicUser(User user) {
            name = user.name;
            username = user.username;
            pronouns = user.pronouns;
            registered = user.registered;

            if (user.emailVisible) {
                email = user.email;
            }
        }
    }
}