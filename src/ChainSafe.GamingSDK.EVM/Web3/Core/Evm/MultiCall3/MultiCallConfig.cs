using System.Collections.Generic;

namespace ChainSafe.GamingSDK.EVM.Web3.Core.Evm.MultiCall3
{
    public class MultiCallConfig
    {
        public MultiCallConfig(Dictionary<string, string> customNetworks)
        {
            CustomNetworks = customNetworks;
        }

        public IReadOnlyDictionary<string, string> CustomNetworks { get; }
    }
}