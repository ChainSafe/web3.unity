using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.Mud.Tables;
using ChainSafe.Gaming.RPC.Events;
using ChainSafe.Gaming.Web3.Core.Nethereum;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Mud;
using Nethereum.Mud.Contracts.Core.StoreEvents;
using Nethereum.Mud.EncodingDecoding;
using Nethereum.Mud.TableRepository;
using Nethereum.Util;

namespace ChainSafe.Gaming.Mud.Storages.InMemory
{
    public class InMemoryMudStorage : IMudStorage
    {
        private readonly INethereumWeb3Adapter nWeb3;
        private readonly EventManager eventManager;

        private readonly SemaphoreSlim storeUpdateSemaphore = new(1);

        private IInMemoryMudStorageConfig config;
        private InMemoryTableRepository inMemoryRepository;

        public InMemoryMudStorage(INethereumWeb3Adapter nWeb3, EventManager eventManager)
        {
            this.eventManager = eventManager;
            this.nWeb3 = nWeb3;
        }

        public event RecordSetDelegate RecordSet;

        public event RecordDeletedDelegate RecordDeleted;

        public async Task Initialize(IMudStorageConfig mudStorageConfig, string worldAddress)
        {
            config = (IInMemoryMudStorageConfig)mudStorageConfig;
            inMemoryRepository = new InMemoryTableRepository();
            var storeLogProcessingService = new StoreEventsLogProcessingService(nWeb3, worldAddress);
            await storeLogProcessingService.ProcessAllStoreChangesAsync(
                inMemoryRepository,
                config.FromBlockNumber,
                null,
                CancellationToken.None);

            await eventManager.Subscribe<StoreSetRecordEventDTO>(OnStoreSetRecord);
            await eventManager.Subscribe<StoreSpliceStaticDataEventDTO>(OnStoreSpliceStaticData);
            await eventManager.Subscribe<StoreSpliceDynamicDataEventDTO>(OnStoreSpliceDynamicDataEventDTO);
            await eventManager.Subscribe<StoreDeleteRecordEventDTO>(OnStoreDeleteRecord);
        }

        public async Task Terminate()
        {
            await eventManager.Unsubscribe<StoreSetRecordEventDTO>(OnStoreSetRecord);
            await eventManager.Unsubscribe<StoreSpliceStaticDataEventDTO>(OnStoreSpliceStaticData);
            await eventManager.Unsubscribe<StoreSpliceDynamicDataEventDTO>(OnStoreSpliceDynamicDataEventDTO);
            await eventManager.Unsubscribe<StoreDeleteRecordEventDTO>(OnStoreDeleteRecord);
        }

        public async Task<object[][]> Query(MudTableSchema tableSchema, MudQuery query)
        {
            var columnParameters = tableSchema.ColumnsToValueParametersOutput().ToArray();
            var encodedRecords = await inMemoryRepository.GetRecordsAsync(tableSchema.ResourceId);

            var rawRecords = encodedRecords.Select(ToRawRecord);
            var filteredRecords = Filter(tableSchema, rawRecords, query);

            return filteredRecords.ToArray();

            object[] ToRawRecord(EncodedTableRecord encodedRecord)
            {
                var encodedValues = encodedRecord.EncodedValues;
                var encodedBytes = ByteUtil.Merge(encodedValues.StaticData)
                    .Concat(encodedValues.EncodedLengths)
                    .Concat(ByteUtil.Merge(encodedValues.DynamicData))
                    .ToArray();

                var resultParameters = ValueEncoderDecoder.DecodeValues(encodedBytes, columnParameters);
                return resultParameters.Select(p => p.Result).ToArray();
            }
        }

        private static IEnumerable<object[]> Filter(
            MudTableSchema tableSchema,
            IEnumerable<object[]> rawRecords,
            MudQuery query)
        {
            if (query.FindWithKey)
            {
                var keyIndices = tableSchema.KeyIndices;
                var record = rawRecords.SingleOrDefault(record => KeyEquals(record, keyIndices, query.KeyFilter));

                if (record != null)
                {
                    return new[] { record };
                }
                else
                {
                    return Array.Empty<object[]>();
                }

                bool KeyEquals(object[] record, int[] keyColumnIndices, object[] keys)
                {
                    if (keyColumnIndices.Length == 0)
                    {
                        throw new InvalidOperationException($"{nameof(keyColumnIndices)} is empty");
                    }

                    for (var keyIndex = 0; keyIndex < keyColumnIndices.Length; keyIndex++)
                    {
                        var columnIndex = keyColumnIndices[keyIndex];

                        if (!record[columnIndex].Equals(keys[keyIndex]))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            // fallback: return original
            return rawRecords;
        }

        private async Task<bool> RecordExists(byte[] tableId, List<byte[]> keyTuple)
        {
            var existingRecord = await inMemoryRepository.GetRecordAsync(
                tableId.ToHex(true),
                InMemoryTableRepository.ConvertKeyToCommaSeparatedHex(keyTuple));
            var recordExists = existingRecord != null;
            return recordExists;
        }

        private async void OnStoreSetRecord(StoreSetRecordEventDTO obj)
        {
            var recordExists = await RecordExists(obj.TableId, obj.KeyTuple);

            await storeUpdateSemaphore.WaitAsync();
            try
            {
                await inMemoryRepository.SetRecordAsync(obj.TableId, obj.KeyTuple, obj.StaticData, obj.EncodedLengths, obj.DynamicData);
            }
            finally
            {
                storeUpdateSemaphore.Release();
            }

            RecordSet.Invoke(obj.TableId, obj.KeyTuple, !recordExists);
        }

        private async void OnStoreSpliceStaticData(StoreSpliceStaticDataEventDTO obj)
        {
            var recordExists = await RecordExists(obj.TableId, obj.KeyTuple);

            await storeUpdateSemaphore.WaitAsync();
            try
            {
                await inMemoryRepository.SetSpliceStaticDataAsync(obj.TableId, obj.KeyTuple, obj.Start, obj.Data);
            }
            finally
            {
                storeUpdateSemaphore.Release();
            }

            RecordSet.Invoke(obj.TableId, obj.KeyTuple, !recordExists);
        }

        private async void OnStoreSpliceDynamicDataEventDTO(StoreSpliceDynamicDataEventDTO obj)
        {
            var recordExists = await RecordExists(obj.TableId, obj.KeyTuple);

            await storeUpdateSemaphore.WaitAsync();
            try
            {
                await inMemoryRepository.SetSpliceDynamicDataAsync(obj.TableId, obj.KeyTuple, obj.Start, obj.Data, obj.DeleteCount, obj.EncodedLengths);
            }
            finally
            {
                storeUpdateSemaphore.Release();
            }

            RecordSet.Invoke(obj.TableId, obj.KeyTuple, !recordExists);
        }

        private async void OnStoreDeleteRecord(StoreDeleteRecordEventDTO obj)
        {
            await storeUpdateSemaphore.WaitAsync();
            try
            {
                await inMemoryRepository.DeleteRecordAsync(obj.TableId, obj.KeyTuple);
            }
            finally
            {
                storeUpdateSemaphore.Release();
            }

            RecordDeleted.Invoke(obj.TableId, obj.KeyTuple);
        }
    }
}