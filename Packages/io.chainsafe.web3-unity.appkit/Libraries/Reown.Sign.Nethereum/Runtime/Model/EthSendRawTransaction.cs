using System.Collections.Generic;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace Reown.Sign.Nethereum.Model
{
    [RpcMethod("eth_sendRawTransaction")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99996)]
    public class EthSendRawTransaction : List<string>
    {
        public EthSendRawTransaction(string transaction) : base(new[] { transaction })
        {
        }

        [Preserve]
        public EthSendRawTransaction()
        {
        }
    }
}