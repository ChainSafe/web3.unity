using System.Collections.Generic;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace Reown.Sign.Nethereum.Model
{
    [RpcMethod("eth_signTypedData_v4")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99999)]
    public class EthSignTypedDataV4 : List<string>
    {
        public EthSignTypedDataV4(string account, string data) : base(new[] { account, data })
        {
        }

        [Preserve]
        public EthSignTypedDataV4()
        {
        }
    }
}