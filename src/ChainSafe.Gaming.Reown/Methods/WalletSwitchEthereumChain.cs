using System.Collections.Generic;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace ChainSafe.Gaming.Reown.Methods
{
    /// <summary>
    /// Switch Ethereum Chain Reown Json RPC method params.
    /// </summary>
    [RpcMethod("wallet_switchEthereumChain")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99995)]
    public class WalletSwitchEthereumChain : List<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WalletSwitchEthereumChain"/> class.
        /// </summary>
        /// <param name="chainId">The chain ID to switch to, in hex format.</param>
        public WalletSwitchEthereumChain(object chainId)
            : base(new[]
            {
                chainId,
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletSwitchEthereumChain"/> class used by json.net.
        /// Preserved for Unity using ChainSafe.Gaming.Unity/link.xml.
        /// </summary>
        public WalletSwitchEthereumChain()
        {
        }
    }
}