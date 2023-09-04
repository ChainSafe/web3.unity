using System;

namespace Web3Unity.Scripts.Library.Ethers.Providers
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