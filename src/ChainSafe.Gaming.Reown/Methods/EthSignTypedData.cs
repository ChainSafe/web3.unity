using System.Collections.Generic;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace ChainSafe.Gaming.Reown.Methods
{
    /// <summary>
    /// Sign Typed Data Reown Json RPC method params.
    /// </summary>
    [RpcMethod("eth_signTypedData")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99998)]
    public class EthSignTypedData : List<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EthSignTypedData"/> class.
        /// </summary>
        /// <param name="address">Public address of signer.</param>
        /// <param name="typedData">SerializableTypedData.</param>
        public EthSignTypedData(string address, object typedData)
            : base(new object[]
            {
                address,
                typedData,
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthSignTypedData"/> class used by json.net.
        /// Preserved for Unity using ChainSafe.Gaming.Unity/link.xml.
        /// </summary>
        public EthSignTypedData()
        {
        }
    }
}