using Degate.Services;

namespace Degate.Config {
    public interface IDegateContext {
        IRemoteTokenResolver sessionTokenResolver { get; }
    }
}