using System;
using System.Net.Http;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.JsonRpc.Client;
using Nethereum.Unity.Rpc;

namespace ChainSafe.Gaming.InProcessTransactionExecutor.Unity
{
    public class RpcClientWrapper : IRpcClientWrapper
    {
        private readonly IChainConfig chainConfig;

        public RpcClientWrapper(IChainConfig chainConfig)
        {
            this.chainConfig = chainConfig;
        }

        public IClient Client => new RpcClient(new Uri(chainConfig.Rpc));
    }
}