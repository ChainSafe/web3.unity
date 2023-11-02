using System.Collections.Generic;

namespace ChainSafe.Gaming.MultiCall
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