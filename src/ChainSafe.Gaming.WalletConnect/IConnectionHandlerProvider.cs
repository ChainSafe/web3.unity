using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect
{
    public interface IConnectionHandlerProvider
    {
        Task<IConnectionHandler> ProvideHandler();
    }
}