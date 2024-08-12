using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Mud.Storages;

namespace ChainSafe.Gaming.Mud.Worlds
{
    public class MudWorldFactory
    {
        private readonly IMudStorageFactory storageFactory;
        private readonly IMudStorageConfig defaultStorageConfig;
        private IContractBuilder contractBuilder;

        public MudWorldFactory(IMudStorageFactory storageFactory, IMudConfig mudConfig, IContractBuilder contractBuilder)
        {
            this.contractBuilder = contractBuilder;
            defaultStorageConfig = mudConfig.StorageConfig;
            this.storageFactory = storageFactory;
        }

        public async Task<MudWorld> Build(MudWorldConfig config)
        {
            var storageConfig = config.StorageConfigOverride ?? defaultStorageConfig;
            var storage = await storageFactory.Build(storageConfig, config.ContractAddress);
            return new MudWorld(config, storage, contractBuilder);
        }
    }
}