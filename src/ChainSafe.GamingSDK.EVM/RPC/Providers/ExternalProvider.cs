using System.Threading.Tasks;

namespace ChainSafe.GamingWeb3.Evm.Providers
{
    public interface IExternalProvider
    {
        // public bool IsMetaMask { get; }
        // public string GetHost();
        public string GetPath();

        public Task<T> Send<T>(string method, object[] parameters = null);
    }
}