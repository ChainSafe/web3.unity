using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Mud.Storages;
using ChainSafe.Gaming.Web3.Environment;

namespace ChainSafe.Gaming.Mud.Worlds
{
    public class MudWorldFactory
    {
        private readonly IMudStorageFactory storageFactory;
        private readonly IMudStorageConfig defaultStorageConfig;
        private readonly IContractBuilder contractBuilder;
        private readonly IMainThreadRunner mainThreadRunner;

        public MudWorldFactory(IMudStorageFactory storageFactory, IMudConfig mudConfig, IContractBuilder contractBuilder, IMainThreadRunner mainThreadRunner)
        {
            this.mainThreadRunner = mainThreadRunner;
            this.contractBuilder = contractBuilder;
            defaultStorageConfig = mudConfig.StorageConfig;
            this.storageFactory = storageFactory;
        }

        public async Task<MudWorld> Build(IMudWorldConfig config)
        {
            var storageConfig = config.StorageConfigOverride ?? defaultStorageConfig;
            var storage = await storageFactory.Build(storageConfig, config.ContractAddress);
            return new MudWorld(config, storage, contractBuilder, mainThreadRunner);
        }
    }
}