using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainSafe.Gaming.Mud.Draft
{
    public class MudWorldTables
    {
        private readonly IMudStorage storage;
        private readonly List<MudTable> tables;

        public MudWorldTables(List<MudTableSchema> tableSchemas, IMudStorage storage)
        {
            this.storage = storage;
            tables = tableSchemas.Select(BuildTable).ToList();
        }

        public MudTable GetTable(byte[] resourceId)
        {
            return tables.Single(table => table.ResourceId.AsSpan().SequenceEqual(resourceId.AsSpan()));
        }

        public MudTable GetTable(string @namespace, string tableName)
        {
            return GetTable(MudUtils.TableResourceId(@namespace, tableName));
        }

        private MudTable BuildTable(MudTableSchema tableSchema)
        {
            return new MudTable(tableSchema, storage);
        }
    }
}