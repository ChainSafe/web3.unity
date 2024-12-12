using System.Collections.Generic;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace Reown.Sign.Nethereum.Model
{
    [RpcMethod("eth_sendTransaction")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99997)]
    public class EthSendTransaction : List<Transaction>
    {
        public EthSendTransaction(params Transaction[] transactions) : base(transactions)
        {
        }

        [Preserve]
        public EthSendTransaction()
        {
        }
    }
}