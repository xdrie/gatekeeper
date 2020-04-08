using Degate.Services;
using Gatekeeper.Remote;

namespace Degate.Config {
    public interface IDegateContext {
        IAuthSessionResolver authSessionResolver { get; }
        IRemoteAuthClient remoteAuthClient { get; }
    }
}