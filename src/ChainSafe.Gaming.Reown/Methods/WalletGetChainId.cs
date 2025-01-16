using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace ChainSafe.Gaming.Reown.Methods
{
    [RpcMethod("eth_chainId")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99998)]
    public class WalletGetChainId
    {
        public WalletGetChainId(params object[] obj)
        {
        }
    }
}