using System;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc.Providers
{
    [Serializable]
    public class RpcConfig
    {
        /// <summary>
        /// (Optional) Url of RPC Node.
        /// </summary>
        public string RpcNodeUrl { get; set; }
    }
}