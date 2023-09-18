using System.Threading.Tasks;

namespace ChainSafe.Gaming.Evm.Providers
{
    public interface IRpcProvider
    {
        Network.Network LastKnownNetwork { get; }

        // Network
        Task<Network.Network> RefreshNetwork();

        // ENS
        Task<T> Perform<T>(string method, params object[] parameters);
    }
}