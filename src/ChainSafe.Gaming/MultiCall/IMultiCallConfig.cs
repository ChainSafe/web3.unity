using System.Collections.Generic;

namespace ChainSafe.Gaming.MultiCall
{
    /// <summary>
    /// Represents a configuration interface for the MultiCall service.
    /// </summary>
    public interface IMultiCallConfig
    {
        /// <summary>
        /// Gets a dictionary of custom network addresses for the MultiCall service.
        /// This allows specifying custom MultiCall contract addresses for specific Ethereum networks.
        /// </summary>
        /// <remarks>
        /// The keys in the dictionary represent Ethereum network identifiers (e.g., chain IDs), and the values
        /// represent the corresponding MultiCall contract addresses for those networks.
        /// </remarks>
        public IReadOnlyDictionary<string, string> CustomNetworks { get; }
    }
}