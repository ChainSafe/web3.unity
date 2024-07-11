﻿using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nethereum.JsonRpc.Client;

namespace ChainSafe.Gaming.Evm.JsonRpc
{
    public static class RpcClientExtensions
    {
        /// <summary>
        /// Binds JSON RPC implementation of EVM Provider to Web3.
        /// </summary>
        /// <returns>The same service collection that was passed in. This enables fluent style.</returns>
        public static IWeb3ServiceCollection UseRpcProvider(this IWeb3ServiceCollection collection)
        {
            collection.AssertServiceNotBound<IRpcProvider>();
            collection.AddSingleton<IRpcProvider, ILifecycleParticipant, RpcClientProvider>();

            collection.AssertServiceNotBound<IClient>();
            collection.AddSingleton<IClient, RpcClientProvider>();
            return collection;
        }
    }
}