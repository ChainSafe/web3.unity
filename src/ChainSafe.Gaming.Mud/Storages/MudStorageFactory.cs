using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.Mud.Storages
{
    public class MudStorageFactory : IMudStorageFactory
    {
        private readonly IServiceProvider serviceProvider;

        public MudStorageFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<IMudStorage> Build(IMudStorageConfig mudStorageConfig, string worldAddress)
        {
            if (!typeof(IMudStorage).IsAssignableFrom(mudStorageConfig.StorageStrategyType))
            {
                throw new MudException($"Provided MUD Storage Strategy type doesn't implement {nameof(IMudStorage)}");
            }

            var storageStrategy = (IMudStorage)serviceProvider.GetRequiredService(mudStorageConfig.StorageStrategyType);
            await storageStrategy.Initialize(mudStorageConfig, worldAddress);
            return storageStrategy;
        }
    }
}