#nullable enable
using System;
using System.Collections.Generic;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3
{
    [Serializable]
    public class MultiCallConfig
    {
        public Dictionary<string, string> CustomNetworks;

        public MultiCallConfig(Dictionary<string, string> customNetworks)
        {
            this.CustomNetworks = customNetworks;
        }
    }
}