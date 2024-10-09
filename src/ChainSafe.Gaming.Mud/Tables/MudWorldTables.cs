using System;
using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.Mud.Storages;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Mud.Tables
{
    public class MudWorldTables
    {
        private readonly IMudStorage storage;
        private readonly List<MudTable> tables;
        private readonly IMainThreadRunner mainThreadRunner;

        public MudWorldTables(List<MudTableSchema> tableSchemas, IMudStorage storage, IMainThreadRunner mainThreadRunner)
        {
            this.mainThreadRunner = mainThreadRunner;
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
            return new MudTable(tableSchema, storage, mainThreadRunner);
        }
    }
}