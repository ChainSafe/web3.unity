﻿using System.Collections.Generic;
using ChainSafe.Gaming.Evm.Network;
using Reown.Core.Common.Utils;
using Reown.Core.Network.Models;

namespace ChainSafe.Gaming.Reown.Methods
{
    [RpcMethod("wallet_addEthereumChain")]
    [RpcRequestOptions(Clock.ONE_MINUTE, 99993)]
    public class WalletAddEthereumChain : List<object>
    {
        public WalletAddEthereumChain(object[] chains)
            : base(chains)
        {
        }

        public WalletAddEthereumChain()
        {
        }
    }
}