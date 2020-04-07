using Gatekeeper.Models.Identity;

namespace Degate.Services {
    public interface IAuthSessionResolver {
        string issueSession(RemoteAuthentication identity);
        RemoteAuthentication resolveSessionToken(string token);
        string getSessionToken(string userId);
        string getUserId(string sessionToken);
    }
}