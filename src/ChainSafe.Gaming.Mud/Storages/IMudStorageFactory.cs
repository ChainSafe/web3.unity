using System.Threading.Tasks;

namespace ChainSafe.Gaming.Mud.Storages
{
    public interface IMudStorageFactory
    {
        Task<IMudStorage> Build(IMudStorageConfig mudStorageConfig, string worldAddress);
    }
}