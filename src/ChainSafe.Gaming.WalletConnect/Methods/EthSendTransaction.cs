using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace ChainSafe.Gaming.WalletConnect.Methods
{
    /// <summary>
    /// Send Transaction Wallet Connect Json RPC method params.
    /// </summary>
    [RpcMethod("eth_sendTransaction")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99999)]
    public class EthSendTransaction : List<TransactionModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EthSendTransaction"/> class.
        /// </summary>
        /// <param name="transactions">Transaction to be sent.</param>
        public EthSendTransaction(params TransactionModel[] transactions)
            : base(transactions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EthSendTransaction"/> class used by json.net.
        /// Preserved for Unity using ChainSafe.Gaming.Unity/link.xml.
        /// </summary>
        public EthSendTransaction()
        {
        }
    }
}