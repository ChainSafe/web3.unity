using System.Threading.Tasks;

namespace Web3Unity.Scripts.Library.Ethers.Providers
{
    public interface IRpcProvider
    {
        Network.Network Network { get; }

        // Network
        Task<Network.Network> DetectNetwork();

        Task<Network.Network> GetNetwork();

        // ENS
        Task<T> Perform<T>(string method, params object[] parameters);
    }
}