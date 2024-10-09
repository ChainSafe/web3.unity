using System;
using System.Numerics;

namespace ChainSafe.Gaming.Mud.Storages.InMemory
{
    public interface IInMemoryMudStorageConfig : IMudStorageConfig
    {
        Type IMudStorageConfig.StorageStrategyType => typeof(InMemoryMudStorage);

        BigInteger FromBlockNumber { get; }
    }
}