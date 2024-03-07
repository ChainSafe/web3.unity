using System;
using System.Net.Http;
using ChainSafe.Gaming.Web3;
using Nethereum.JsonRpc.Client;
using Nethereum.Unity.Rpc;

namespace ChainSafe.Gaming.InProcessTransactionExecutor.Unity
{
    public class RpcClientWrapper : IRpcClientWrapper
    {
        public RpcClientWrapper(IChainConfig chainConfig)
        {
            Client = new SimpleRpcClient(new Uri(chainConfig.Rpc), new HttpClient());
        }

        public IClient Client { get; private set; }
    }
}