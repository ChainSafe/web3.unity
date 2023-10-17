using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace ChainSafe.Gaming.WalletConnect.Methods
{
    [RpcMethod("eth_sendTransaction")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99999)]
    public class EthSendTransaction : List<TransactionModel>
    {
        public EthSendTransaction(params TransactionModel[] transactions)
            : base(transactions)
        {
        }

        public EthSendTransaction()
        {
        }
    }
}