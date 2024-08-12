using System;

namespace ChainSafe.Gaming.Mud.Storages
{
    public interface IMudStorageConfig
    {
        Type StorageStrategyType { get; }
    }
}