using System.Threading.Tasks;

namespace ChainSafe.Gaming.WalletConnect.Connection
{
    /// <summary>
    /// Provider of <see cref="IConnectionHandler"/>.
    /// </summary>
    public interface IConnectionHandlerProvider
    {
        Task<IConnectionHandler> ProvideHandler();
    }
}