using System.Threading.Tasks;
using ChainSafe.Gaming.Mud.Tables;

namespace ChainSafe.Gaming.Mud.Storages
{
    public interface IMudStorage
    {
        Task Initialize(IMudStorageConfig mudStorageConfig, string worldAddress);

        Task Terminate();

        Task<object[][]> Query(MudTableSchema tableSchema, MudQuery query);
    }
}