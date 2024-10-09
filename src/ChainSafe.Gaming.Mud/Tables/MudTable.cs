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
    /// <summary>
    /// Represents a table of a MUD world.
    /// </summary>
    public class MudTable // todo support LINQ or IQueryable
    {
        private readonly IMudStorage storage;
        private readonly MudTableSchema tableSchema;
        private readonly IMainThreadRunner mainThreadRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="MudTable"/> class.
        /// </summary>
        /// <param name="tableSchema">The schema of the table.</param>
        /// <param name="storage">The storage implementation.</param>
        /// <param name="mainThreadRunner">The main thread runner implementation.</param>
        public MudTable(MudTableSchema tableSchema, IMudStorage storage, IMainThreadRunner mainThreadRunner)
        {
            this.mainThreadRunner = mainThreadRunner;
            this.tableSchema = tableSchema;
            this.storage = storage;

            storage.RecordSet += OnStorageRecordSet;
            storage.RecordDeleted += OnStorageRecordDeleted;
        }

        /// <summary>
        /// Event that is raised when a record is added to a table.
        /// </summary>
        /// <remarks>
        /// This event is triggered when a new record is added to the table. The event handler will be invoked with information about the added record.
        /// </remarks>
        public event RecordSetDelegate? RecordAdded;

        /// <summary>
        /// Represents an event that is raised when a record has been updated.
        /// </summary>
        /// <remarks>
        /// The RecordUpdated event is typically used to notify subscribers that a record has been updated in a MUD table.
        /// </remarks>
        public event RecordSetDelegate? RecordUpdated;

        /// <summary>
        /// Event that is raised when a record is removed.
        /// </summary>
        public event RecordDeletedDelegate? RecordRemoved;

        /// <summary>
        /// Gets the resource identifier as a byte array.
        /// </summary>
        /// <value>
        /// The resource identifier as a byte array.
        /// </value>
        public byte[] ResourceId => tableSchema.ResourceId;

        /// <summary>
        /// Executes a query on the storage using the specified MudQuery object.
        /// </summary>
        /// <param name="query">The MudQuery object representing the query to be executed.</param>
        /// <returns>
        /// A Task containing an array of records that represent the results of the query.
        /// Each sub-array element represents values for each column of the record.
        /// </returns>
        public Task<object[][]> Query(MudQuery query)
        {
            return storage.Query(tableSchema, query);
        }

        /// <summary>
        /// Queries the database for a single object based on the given query.
        /// Ensures there is only one element that fits the query.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an array that contains values of each column of the record.
        /// </returns>
        /// <exception cref="InvalidOperationException">The query result contains more or less than 1 element.</exception>
        public Task<object[]> QuerySingle(MudQuery query)
        {
            return storage.QuerySingle(tableSchema, query);
        }

        /// <summary>
        /// Queries the storage for a single object based on the specified key.
        /// </summary>
        /// <param name="key">The key used to query the object.</param>
        /// <returns>
        /// An awaitable task that represents the asynchronous operation. The task result contains an array that represents values of each column of the record.
        /// </returns>
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