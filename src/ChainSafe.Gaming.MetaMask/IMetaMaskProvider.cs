using System.Threading.Tasks;

namespace ChainSafe.Gaming.MetaMask
{
    public interface IMetaMaskProvider
    {
        public Task<string> Connect();

        public Task<T> Request<T>(string method, params object[] parameters);

        public Task Disconnect();
    }
}