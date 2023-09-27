using System;

namespace ChainSafe.Gaming.Evm.Providers
{
    [Serializable]
    public class RpcClientConfig
    {
        /// <summary>
        /// (Optional) Url of RPC Node.
        /// </summary>
        public string RpcNodeUrl { get; set; }
    }
}