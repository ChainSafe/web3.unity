using System.Collections.Generic;
using ChainSafe.Gaming.WalletConnect.Models;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace ChainSafe.Gaming.WalletConnect.Methods
{
    [RpcMethod("eth_signTransaction")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99996)]
    public class EthSignTransaction : List<TransactionModel>
    {
        public EthSignTransaction(params TransactionModel[] transactions)
            : base(transactions)
        {
        }

        public EthSignTransaction()
        {
        }
    }
}