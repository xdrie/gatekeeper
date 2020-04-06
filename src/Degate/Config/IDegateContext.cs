using Degate.Services;
using Gatekeeper.Remote;

namespace Degate.Config {
    public interface IDegateContext {
        IRemoteTokenResolver sessionTokenResolver { get; }
        GateAuthClient gateAuthClient { get; }
    }
}