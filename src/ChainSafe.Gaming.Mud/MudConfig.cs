using ChainSafe.Gaming.Mud.Storages;

namespace ChainSafe.Gaming.Mud
{
    public class MudConfig : IMudConfig
    {
        public IMudStorageConfig StorageConfig { get; set; }
    }
}