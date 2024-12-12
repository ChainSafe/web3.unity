using System.Collections.Generic;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace Reown.Sign.Nethereum.Model
{
    [RpcMethod("wallet_switchEthereumChain")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99990)]
    public class WalletSwitchEthereumChain : List<object>
    {
        public WalletSwitchEthereumChain(string chainId) : base(new[] { new { chainId } })
        {
        }

        [Preserve]
        public WalletSwitchEthereumChain()
        {
        }
    }
}