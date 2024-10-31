using System.Collections.Generic;

namespace ChainSafe.Gaming.MultiCall
{
    public class MultiCallConfig
    {
        public MultiCallConfig(Dictionary<string, string> customNetworks)
        {
            CustomNetworks = customNetworks;
        }

        public enum UnavailableBehaviourType
        {
            Throw,
            DisableAndLog,
        }

        /// <summary>
        /// Contract address of the MultiCall contract by Chain ID.
        /// </summary>
        public IReadOnlyDictionary<string, string> CustomNetworks { get; }

        public UnavailableBehaviourType UnavailableBehaviour { get; set; } = UnavailableBehaviourType.DisableAndLog;
    }
}