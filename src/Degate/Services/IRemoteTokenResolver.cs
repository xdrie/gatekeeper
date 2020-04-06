using Gatekeeper.Models.Identity;

namespace Degate.Services {
    public interface IRemoteTokenResolver {
        RemoteAuthentication resolve(string token);
    }
}