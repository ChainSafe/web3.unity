using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;

namespace ChainSafe.Gaming.Web3.Evm.Wallet
{
    public interface IWalletProvider : IRpcProvider
    {
        Task<string> Connect();

        Task Disconnect();
    }
}