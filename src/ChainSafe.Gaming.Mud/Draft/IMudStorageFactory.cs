using System.Threading.Tasks;

namespace ChainSafe.Gaming.Mud.Draft
{
    public interface IMudStorageFactory
    {
        Task<IMudStorage> Build(IMudStorageConfig mudStorageConfig, string worldAddress);
    }
}