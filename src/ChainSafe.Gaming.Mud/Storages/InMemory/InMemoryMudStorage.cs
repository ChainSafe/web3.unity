using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.Mud.Tables;
using ChainSafe.Gaming.RPC.Events;
using ChainSafe.Gaming.Web3.Core.Nethereum;
using Nethereum.ABI.FunctionEncoding.Attributes;
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
            var columnParameters = tableSchema.ColumnsToParametersOutput().ToArray();
            var encodedRecords = await inMemoryRepository.GetRecordsAsync(tableSchema.ResourceId); // bug returns 0 elements

            var rawRecords = encodedRecords.Select(ToRawRecord); // todo apply 'query' filters here

            return rawRecords.ToArray();

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

        private async void OnStoreSetRecord(StoreSetRecordEventDTO obj)
        {
            await storeUpdateSemaphore.WaitAsync();
            try
            {
                await inMemoryRepository.SetRecordAsync(obj.TableId, obj.KeyTuple, obj.StaticData, obj.EncodedLengths, obj.DynamicData);
            }
            finally
            {
                storeUpdateSemaphore.Release();
            }
        }

        private async void OnStoreSpliceStaticData(StoreSpliceStaticDataEventDTO obj)
        {
            await storeUpdateSemaphore.WaitAsync();
            try
            {
                await inMemoryRepository.SetSpliceStaticDataAsync(obj.TableId, obj.KeyTuple, obj.Start, obj.Data);
            }
            finally
            {
                storeUpdateSemaphore.Release();
            }
        }

        private async void OnStoreSpliceDynamicDataEventDTO(StoreSpliceDynamicDataEventDTO obj)
        {
            await storeUpdateSemaphore.WaitAsync();
            try
            {
                await inMemoryRepository.SetSpliceDynamicDataAsync(obj.TableId, obj.KeyTuple, obj.Start, obj.Data, obj.DeleteCount, obj.EncodedLengths);
            }
            finally
            {
                storeUpdateSemaphore.Release();
            }
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
        }
    }
}