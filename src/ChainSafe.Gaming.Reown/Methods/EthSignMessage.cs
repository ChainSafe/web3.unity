using System.Collections.Generic;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace ChainSafe.Gaming.Reown.Methods
{
    /// <summary>
    /// Sign message Reown Json RPC method params.
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