using System.Threading.Tasks;

namespace ChainSafe.Gaming.Reown.Connection
{
    /// <summary>
    /// Provider of <see cref="IConnectionHandler"/>.
    /// </summary>
    public interface IConnectionHandlerProvider
    {
        Task<IConnectionHandler> ProvideHandler();
    }
}