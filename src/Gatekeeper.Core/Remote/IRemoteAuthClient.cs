using System.Threading.Tasks;
using Gatekeeper.Models.Identity;

namespace Gatekeeper.Remote {
    public interface IRemoteAuthClient {
        bool validate(Token token);
        Task<RemoteAuthentication> getRemoteIdentity(Token token);
    }
}