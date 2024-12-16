using System.Collections.Generic;
using ChainSafe.Gaming.Reown.Models;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace ChainSafe.Gaming.Reown.Methods
{
    /// <summary>
    /// Sign Transaction Reown Json RPC method params.
    /// </summary>
    [RpcMethod("eth_signTransaction")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99996)]
    public class EthSignTransaction : List<TransactionModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EthSignTransaction"/> class.
        /// </summary>
        /// <param name="transactions">Transaction to be signed.</param>
        public EthSignTransaction(params TransactionModel[] transactions)
            : base(transactions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthSignTransaction"/> class used by json.net.
        /// Preserved for Unity using ChainSafe.Gaming.Unity/link.xml.
        /// </summary>
        public EthSignTransaction()
        {
        }
    }
}