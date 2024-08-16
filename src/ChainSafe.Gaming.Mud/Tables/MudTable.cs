using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Mud.Storages;
using ChainSafe.Gaming.Mud.Storages.InMemory;
using ChainSafe.Gaming.Web3.Environment;
using Nethereum.Mud.EncodingDecoding;

namespace ChainSafe.Gaming.Mud.Tables
{
    public class MudTable // todo support LINQ or IQueryable
    {
        private readonly IMudStorage storage;
        private readonly MudTableSchema tableSchema;
        private readonly IMainThreadRunner mainThreadRunner;

        public MudTable(MudTableSchema tableSchema, IMudStorage storage, IMainThreadRunner mainThreadRunner)
        {
            this.mainThreadRunner = mainThreadRunner;
            this.tableSchema = tableSchema;
            this.storage = storage;

            storage.RecordSet += OnStorageRecordSet;
            storage.RecordDeleted += OnStorageRecordDeleted;
        }

        public event RecordSetDelegate? RecordAdded;

        public event RecordSetDelegate? RecordUpdated;

        public event RecordDeletedDelegate? RecordRemoved;

        public byte[] ResourceId => tableSchema.ResourceId;

        public Task<object[][]> Query(MudQuery query)
        {
            return storage.Query(tableSchema, query);
        }

        public Task<object[]> QuerySingle(MudQuery query)
        {
            return storage.QuerySingle(tableSchema, query);
        }

        public Task<object[]> QueryByKey(object[] key)
        {
            return storage.QuerySingle(tableSchema, MudQuery.ByKey(key));
        }

        private async void OnStorageRecordSet(byte[] tableId, List<byte[]> key, bool newRecord)
        {
            if (!tableId.SequenceEqual(tableSchema.ResourceId))
            {
                return;
            }

            var decodedKey = DecodeKey(key);
            var value = decodedKey.Length > 0
                ? await storage.QuerySingle(tableSchema, MudQuery.ByKey(decodedKey))
                : await storage.QuerySingle(tableSchema, MudQuery.All); // singleton

            if (newRecord)
            {
                mainThreadRunner.Enqueue(() => RecordAdded?.Invoke(decodedKey, value));
            }
            else
            {
                mainThreadRunner.Enqueue(() => RecordUpdated?.Invoke(decodedKey, value));
            }
        }

        private void OnStorageRecordDeleted(byte[] tableId, List<byte[]> key)
        {
            if (!tableId.SequenceEqual(tableSchema.ResourceId))
            {
                return;
            }

            var decodedKey = DecodeKey(key);
            mainThreadRunner.Enqueue(() => RecordRemoved?.Invoke(decodedKey));
        }

        private object[] DecodeKey(List<byte[]> key)
        {
            if (key.Count == 0)
            {
                return Array.Empty<object>();
            }

            return KeyEncoderDecoder
                .DecodeKey(key, tableSchema.KeyToParametersOutput().ToArray())
                .Select(output => output.Result)
                .ToArray();
        }
    }
}