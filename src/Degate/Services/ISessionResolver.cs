using Gatekeeper.Models.Identity;

namespace Degate.Services {
    public interface ISessionResolver {
        RemoteAuthentication resolveSessionToken(string token);
    }
}