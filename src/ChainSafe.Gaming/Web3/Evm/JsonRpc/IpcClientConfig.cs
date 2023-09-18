using System;

namespace ChainSafe.Gaming.Evm.Providers
{
    [Serializable]
    public class IpcClientConfig
    {
        /// <summary>
        /// (Optional) Path to the IPC file.
        /// </summary>
        public string IpcPath { get; set; }
    }
}