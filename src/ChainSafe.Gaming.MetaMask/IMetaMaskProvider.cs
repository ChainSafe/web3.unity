using System.Threading.Tasks;

namespace ChainSafe.Gaming.MetaMask
{
    public interface IMetaMaskProvider
    {
        public Task<string> Connect();

        public Task<string> Request<T>(T data, long? expiry = null);

        public Task Disconnect();
    }
}