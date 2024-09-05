using System;

namespace ChainSafe.Gaming.Mud.Storages
{
    /// <summary>
    /// Interface for configuring the MUD storage.
    /// </summary>
    public interface IMudStorageConfig
    {
        /// <summary>
        /// Gets the type of the storage strategy.
        /// </summary>
        /// <value>
        /// The type of the storage strategy.
        /// </value>
        Type StorageStrategyType { get; }
    }
}