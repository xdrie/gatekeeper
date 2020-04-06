using Degate.Services;
using Gatekeeper.Remote;

namespace Degate.Config {
    public interface IDegateContext {
        ISessionResolver sessionResolver { get; }
        GateAuthClient gateAuthClient { get; }
    }
}