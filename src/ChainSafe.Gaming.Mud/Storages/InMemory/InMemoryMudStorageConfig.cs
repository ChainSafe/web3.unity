using System.Numerics;

namespace ChainSafe.Gaming.Mud.Storages.InMemory
{
    /// <summary>
    /// Represents the configuration for in-memory MUD storage.
    /// </summary>
    public class InMemoryMudStorageConfig : IInMemoryMudStorageConfig
    {
        /// <summary>
        /// Gets or sets the starting block number for retrieving data.
        /// </summary>
        /// <remarks>
        /// This property represents the block number from which the data retrieval should start.
        /// The value of this property should be a non-negative integer.
        /// </remarks>
        public BigInteger FromBlockNumber { get; set; }
    }
}