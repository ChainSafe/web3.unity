using ChainSafe.Gaming.Mud.Storages;

namespace ChainSafe.Gaming.Mud
{
    public interface IMudConfig
    {
        IMudStorageConfig StorageConfig { get; }
    }
}