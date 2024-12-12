using System.Collections.Generic;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace Reown.Sign.Nethereum.Model
{
    [RpcMethod("wallet_addEthereumChain")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99990)]
    public class WalletAddEthereumChain : List<object>
    {
        public WalletAddEthereumChain(EthereumChain chain) : base(new[] { chain })
        {
        }

        [Preserve]
        public WalletAddEthereumChain()
        {
        }
    }
}