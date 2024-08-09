using System;

namespace ChainSafe.Gaming.Mud.Draft
{
    public interface IMudStorageConfig
    {
        Type StorageStrategyType { get; }
    }
}