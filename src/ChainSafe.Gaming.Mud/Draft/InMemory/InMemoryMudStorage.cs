using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Core.Nethereum;
using Nethereum.Mud;
using Nethereum.Mud.Contracts.Core.StoreEvents;
using Nethereum.Mud.EncodingDecoding;
using Nethereum.Mud.TableRepository;
using Nethereum.Util;

namespace ChainSafe.Gaming.Mud.Draft.InMemory
{
    public class InMemoryMudStorage : IMudStorage
    {
        private readonly INethereumWeb3Adapter nWeb3;

        private IInMemoryMudStorageConfig config;
        private InMemoryTableRepository inMemoryRepository;

        public InMemoryMudStorage(INethereumWeb3Adapter nWeb3)
        {
            this.nWeb3 = nWeb3;
        }

        public Task Initialize(IMudStorageConfig mudStorageConfig, string worldAddress)
        {
            config = (IInMemoryMudStorageConfig)mudStorageConfig;
            inMemoryRepository = new InMemoryTableRepository();
            var storeLogProcessingService = new StoreEventsLogProcessingService(nWeb3, worldAddress);
            return storeLogProcessingService.ProcessAllStoreChangesAsync(
                inMemoryRepository,
                config.FromBlockNumber,
                null,
                CancellationToken.None);

            // todo subscribe to updates
        }

        public Task Terminate()
        {
            // todo unsubscribe from updates

            return Task.CompletedTask;
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
    }
}