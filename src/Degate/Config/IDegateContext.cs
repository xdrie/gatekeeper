using Degate.Services;
using Gatekeeper.Remote;

namespace Degate.Config {
    public interface IDegateContext {
        ISessionResolver sessionTokenResolver { get; }
        GateAuthClient gateAuthClient { get; }
    }
}