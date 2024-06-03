using System;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.JsonRpc.Client;
using Nethereum.Unity.Rpc;

namespace ChainSafe.Gaming.InProcessTransactionExecutor.Unity
{
    public class RpcClientWrapper : IRpcClientWrapper
    {
        public RpcClientWrapper(IChainConfig chainConfig, IMainThreadRunner mainThreadRunner)
        {
            mainThreadRunner.Enqueue(() =>
            {
                Client = new UnityWebRequestRpcTaskClient(new Uri(chainConfig.Rpc));
            });
        }

        public IClient Client { get; private set; }
    }
}