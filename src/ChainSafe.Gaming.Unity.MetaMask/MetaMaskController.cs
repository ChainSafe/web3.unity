using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm;
using ChainSafe.Gaming.Unity.EthereumWindow;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Unity.MetaMask
{
    /// <summary>
    /// Ethereum Window controller for Metamask browser extension wallet.
    /// </summary>
    public class MetaMaskController : EthereumWindowController
    {
        /// <summary>
        /// Checks if MetaMask browser extension installed.
        /// </summary>
        /// <returns>Is MetaMask installed.</returns>
        [DllImport("__Internal")]
        public static extern bool IsMetaMask();

        /// <summary>
        /// Connects to the MetaMask browser extension wallet.
        /// </summary>
        /// <param name="chainConfig">Chain config for what chain to connect to.</param>
        /// <param name="chainRegistryProvider">List of all known chains with details.</param>
        /// <returns>Connected account address.</returns>
        /// <exception cref="Web3Exception">Throws Exception if connection is unsuccessful.</exception>
        public override Task<string> Connect(IChainConfig chainConfig, ChainRegistryProvider chainRegistryProvider)
        {
            if (!IsMetaMask())
            {
                throw new Web3Exception("MetaMask is not installed.");
            }

            return base.Connect(chainConfig, chainRegistryProvider);
        }
    }
}