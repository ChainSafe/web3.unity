using System;
using ChainSafe.Gaming.Mud.Storages;
using ChainSafe.Gaming.Mud.Storages.InMemory;
using ChainSafe.Gaming.Web3;
using UnityEngine;

namespace ChainSafe.Gaming.Mud.Unity
{
    /// <summary>
    /// Represents a configuration asset for the MUD module.
    /// </summary>
    [CreateAssetMenu(menuName = "ChainSafe/MUD Config", fileName = "MudConfig", order = 0)]
    public class MudConfigAsset : ScriptableObject, IMudConfig
    {
        public MudStorageType StorageType;
        public ulong InMemoryFromBlockNumber;

        public IMudStorageConfig StorageConfig => BuildStorageConfig();

        private IMudStorageConfig BuildStorageConfig()
        {
            switch (StorageType)
            {
                case MudStorageType.LocalStorage:
                    return new InMemoryMudStorageConfig { FromBlockNumber = InMemoryFromBlockNumber };
                case MudStorageType.OffchainIndexer:
                    throw new Web3Exception("Offchain Indexer Storage is not implemented yet.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}