using System;

namespace ChainSafe.Gaming.Web3.Evm.JsonRpc.Providers
{
    [Serializable]
    public class IpcConfig
    {
        /// <summary>
        /// (Optional) Path to the IPC file.
        /// </summary>
        public string IpcPath { get; set; }
    }
}