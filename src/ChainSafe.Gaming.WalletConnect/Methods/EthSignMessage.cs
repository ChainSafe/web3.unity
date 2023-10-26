using System.Collections.Generic;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace ChainSafe.Gaming.WalletConnect.Methods
{
    /// <summary>
    /// Sign message Wallet Connect Json RPC method params.
    /// </summary>
    [RpcMethod("personal_sign")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99997)]
    public class EthSignMessage : List<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EthSignMessage"/> class.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <param name="address">Public Address of signer.</param>
        public EthSignMessage(string message, string address)
            : base(new string[] { message, address })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthSignMessage"/> class used by json.net.
        /// Preserved for Unity using ChainSafe.Gaming.Unity/link.xml.
        /// </summary>
        public EthSignMessage()
        {
        }
    }
}