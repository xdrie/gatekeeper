namespace Gatekeeper.Models.Identity {
    /// <summary>
    /// Represents user information visible to an authenticator
    /// </summary>
    public class AuthenticatedUser : PublicUser {
        public bool totpEnabled { get; set; }
        public User.Role role { get; set; }
        public bool emailVisible { get; set; } = false;

        public AuthenticatedUser() { }

        public AuthenticatedUser(User user) : base(user) {
            totpEnabled = user.totpEnabled;
            
            // email is always visible to authenticator
            email = user.email;
            emailVisible = user.emailVisible;
            role = user.role;
        }
    }
}