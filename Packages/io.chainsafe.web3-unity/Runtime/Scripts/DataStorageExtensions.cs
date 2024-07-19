using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Unity;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Unity;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.UnityPackage
{
    public static class DataStorageExtensions
    {
        public static Task<TStorable> LoadOneTime<TStorable>(this TStorable storable)
            where TStorable : class, IStorable
        {
            return storable.LoadOneTime(DataStorageDependencies(storable));
        }
        
        public static Task SaveOneTime<TStorable>(this TStorable storable)
            where TStorable : class, IStorable
        {
            return storable.SaveOneTime(DataStorageDependencies(storable));
        }
        
        public static void ClearOneTime<TStorable>(this TStorable storable)
            where TStorable : class, IStorable
        {
            storable.ClearOneTime(DataStorageDependencies(storable));
        }

        private static IServiceCollection DataStorageDependencies<TStorable>(TStorable storable)
            where TStorable : class, IStorable
        {
            return new ServiceCollection()
                .AddSingleton<DataStorage>()
                .AddSingleton<IStorable>(storable)
                .AddSingleton<ILogWriter, UnityLogWriter>()
                .AddSingleton<IOperatingSystemMediator, UnityOperatingSystemMediator>()
                .AddSingleton<IMainThreadRunner, UnityDispatcherAdapter>();
        }
    }
}
