using System;
using ChainSafe.Gaming.Web3;
using Nethereum.JsonRpc.Client;
using Nethereum.Unity.Rpc;

namespace ChainSafe.Gaming.InProcessTransactionExecutor.Unity
{
    public class RpcClientWrapper : IRpcClientWrapper
    {
        public RpcClientWrapper(IChainConfig chainConfig)
        {
            Client = new UnityWebRequestRpcTaskClient(new Uri(chainConfig.Rpc));
        }

        public IClient Client { get; private set; }
    }
}