using System;

namespace ChainSafe.Gaming.Evm.JsonRpcProvider
{
    [Serializable]
    public class JsonRpcProviderConfig
    {
        /// <summary>
        /// (Optional) Url of RPC Node.
        /// </summary>
        public string RpcNodeUrl { get; set; }

        /// <summary>
        /// (Optional) Network to operate on.
        /// </summary>
        public Network Network { get; set; }
    }
}