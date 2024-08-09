using System.Threading.Tasks;

namespace ChainSafe.Gaming.Mud.Draft
{
    public class MudWorldFactory
    {
        private readonly IMudStorageFactory storageFactory;
        private readonly IMudStorageConfig defaultStorageConfig;

        public MudWorldFactory(IMudStorageFactory storageFactory, IMudConfig mudConfig)
        {
            defaultStorageConfig = mudConfig.StorageConfig;
            this.storageFactory = storageFactory;
        }

        public async Task<MudWorld> Build(MudWorldConfig config)
        {
            var storageConfig = config.StorageConfigOverride ?? defaultStorageConfig;
            var storage = await storageFactory.Build(storageConfig, config.ContractAddress);
            return new MudWorld(config, storage);
        }
    }
}