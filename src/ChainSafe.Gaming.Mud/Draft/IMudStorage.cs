using System.Threading.Tasks;

namespace ChainSafe.Gaming.Mud.Draft
{
    public interface IMudStorage
    {
        Task Initialize(IMudStorageConfig mudStorageConfig, string worldAddress);

        Task Terminate();

        Task<object[][]> Query(MudTableSchema tableSchema, MudQuery query);
    }
}