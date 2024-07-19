using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.LocalStorage
{
    public static class DataStorageExtensions
    {
        public static async Task<TStorable> LoadOneTime<TStorable>(this TStorable storable, IServiceCollection services)
            where TStorable : class, IStorable
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            DataStorage dataStorage = serviceProvider.GetService<DataStorage>();

            await dataStorage.Load(storable);

            return storable;
        }

        public static async Task SaveOneTime<TStorable>(this TStorable storable, IServiceCollection services)
            where TStorable : class, IStorable
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            DataStorage dataStorage = serviceProvider.GetService<DataStorage>();

            await dataStorage.Save(storable);
        }

        public static void ClearOneTime<TStorable>(this TStorable storable, IServiceCollection services)
            where TStorable : class, IStorable
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            DataStorage dataStorage = serviceProvider.GetService<DataStorage>();

            dataStorage.Clear(storable);
        }
    }
}