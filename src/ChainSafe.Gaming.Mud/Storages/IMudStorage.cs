using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Mud.Storages.InMemory;
using ChainSafe.Gaming.Mud.Tables;

namespace ChainSafe.Gaming.Mud.Storages
{
    public interface IMudStorage
    {
        event RecordSetDelegate RecordSet;

        event RecordDeletedDelegate RecordDeleted;

        Task Initialize(IMudStorageConfig mudStorageConfig, string worldAddress);

        Task Terminate();

        Task<object[][]> Query(MudTableSchema tableSchema, MudQuery query);

        async Task<object[]> QuerySingle(MudTableSchema tableSchema, MudQuery query)
            => (await Query(tableSchema, query)).Single();
    }
}