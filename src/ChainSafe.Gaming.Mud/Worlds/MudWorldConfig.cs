using System;
using System.Collections.Generic;
using ChainSafe.Gaming.Mud.Storages;
using ChainSafe.Gaming.Mud.Tables;

namespace ChainSafe.Gaming.Mud.Worlds
{
    [Serializable]
    public class MudWorldConfig : IMudWorldConfig
    {
        public string ContractAddress { get; set; }

        public string ContractAbi { get; set; }

        public string? DefaultNamespace { get; set; }

        public IMudStorageConfig? StorageConfigOverride { get; set; }

        public List<MudTableSchema> TableSchemas { get; set; }
    }
}