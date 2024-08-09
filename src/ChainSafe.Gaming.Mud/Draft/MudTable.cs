using System.Threading.Tasks;

namespace ChainSafe.Gaming.Mud.Draft
{
    public class MudTable // todo make this IQueryable
    {
        private readonly IMudStorage storage;
        private readonly MudTableSchema tableSchema;

        public MudTable(MudTableSchema tableSchema, IMudStorage storage)
        {
            this.tableSchema = tableSchema;
            this.storage = storage;
        }

        public event MudTableMutationDelegate RecordAdded;

        public event MudTableMutationDelegate RecordUpdated;

        public event MudTableMutationDelegate RecordRemoved;

        public byte[] ResourceId => tableSchema.ResourceId;

        public Task<object[][]> Query(MudQuery query) // todo remove MudQueryResponse wrapper?
        {
            return storage.Query(tableSchema, query);
        }
    }
}