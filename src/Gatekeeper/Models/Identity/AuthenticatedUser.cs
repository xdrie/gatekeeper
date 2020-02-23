namespace Gatekeeper.Models.Identity {
    /// <summary>
    /// Represents user information visible to an authenticator
    /// </summary>
    public class AuthenticatedUser : PublicUser {
        public User.Role role { get; set; }

        public AuthenticatedUser() { }

        public AuthenticatedUser(User user) : base(user) {
            // email is always visible to authenticator
            email = user.email;
            role = user.role;
        }
    }
}