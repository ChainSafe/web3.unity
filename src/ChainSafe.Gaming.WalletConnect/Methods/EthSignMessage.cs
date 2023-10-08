using System.Collections.Generic;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace ChainSafe.Gaming.WalletConnect.Methods
{
    [RpcMethod("personal_sign")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99997)]
    public class EthSignMessage : List<string>
    {
        public EthSignMessage(string message, string address)
            : base(new string[] { message, address })
        {
        }

        public EthSignMessage()
        {
        }
    }
}