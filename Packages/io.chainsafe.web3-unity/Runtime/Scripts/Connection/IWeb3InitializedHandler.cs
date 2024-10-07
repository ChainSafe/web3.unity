using System.Threading.Tasks;
using CWeb3 = ChainSafe.Gaming.Web3.Web3;

namespace ChainSafe.Gaming.UnityPackage.Connection
{
    /// <summary>
    /// Initialized handler used for executing logic when a web3 instance is initialized.
    /// </summary>
    public interface IWeb3InitializedHandler
    {
        /// <summary>
        /// Called when Web3 Instance in <see cref="IConnectionHandler"/> is initialized.
        /// </summary>
        public Task OnWeb3Initialized(CWeb3 web3);
    }
}
