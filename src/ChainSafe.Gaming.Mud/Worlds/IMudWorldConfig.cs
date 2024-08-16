using System.Collections.Generic;
using ChainSafe.Gaming.Mud.Storages;
using ChainSafe.Gaming.Mud.Tables;

namespace ChainSafe.Gaming.Mud.Worlds
{
    public interface IMudWorldConfig
    {
        string ContractAddress { get; }

        string ContractAbi { get; }

        string? DefaultNamespace { get; }

        IMudStorageConfig? StorageConfigOverride { get; }

        List<MudTableSchema> TableSchemas { get; }
    }
}