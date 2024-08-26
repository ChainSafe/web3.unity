using System.Collections.Generic;
using ChainSafe.Gaming.Mud.Storages;
using ChainSafe.Gaming.Mud.Tables;

namespace ChainSafe.Gaming.Mud.Worlds
{
    /// <summary>
    /// Represents the configuration for the MUD World.
    /// </summary>
    public interface IMudWorldConfig
    {
        /// <summary>
        /// Gets the MUD world contract address.
        /// </summary>
        /// <value>
        /// The contract address as a string.
        /// </value>
        string ContractAddress { get; }

        /// <summary>
        /// Gets the Contract Application Binary Interface (ABI).
        /// </summary>
        /// <value>
        /// The contract ABI, a JSON representation of the smart contract's interface.
        /// </value>
        string ContractAbi { get; }

        /// <summary>
        /// Gets the default namespace to be used by the MUD World.
        /// </summary>
        /// <value>
        /// The default namespace used by the MUD World or null if not specified.
        /// </value>
        string? DefaultNamespace { get; }

        /// <summary>
        /// Gets the storage configuration override for the MUD World.
        /// </summary>
        /// <returns>
        /// An instance of the IMudStorageConfig interface representing the storage configuration override for the MUD World. Returns null if no override is set.
        /// </returns>
        IMudStorageConfig? StorageConfigOverride { get; }

        /// <summary>
        /// Gets the list of table schemas.
        /// </summary>
        /// <remarks>
        /// This property returns a List of MudTableSchema objects that represent the schemas of the tables.
        /// </remarks>
        /// <returns>
        /// A List of MudTableSchema objects.
        /// </returns>
        List<MudTableSchema> TableSchemas { get; }
    }
}