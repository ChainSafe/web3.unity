using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect.Connection
{
    public interface IConnectionHandlerProvider
    {
        Task<IConnectionHandler> ProvideHandler();
    }
}