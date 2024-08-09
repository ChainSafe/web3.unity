using System;
using System.Numerics;
using ChainSafe.Gaming.Mud.Draft.InMemory;

namespace ChainSafe.Gaming.Mud.Draft
{
    public interface IInMemoryMudStorageConfig : IMudStorageConfig
    {
        Type IMudStorageConfig.StorageStrategyType => typeof(InMemoryMudStorage);

        BigInteger FromBlockNumber { get; }
    }
}