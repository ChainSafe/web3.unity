using System.Collections.Generic;

namespace ChainSafe.Gaming.Mud.Draft
{
    public class MudWorldConfig
    {
        public string ContractAddress { get; set; }

        public string ContractAbi { get; set; }

        public IMudStorageConfig? StorageConfigOverride { get; set; }

        public List<MudTableSchema> TableSchemas { get; set; }
    }
}