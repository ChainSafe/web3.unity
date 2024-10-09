using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using ChainSafe.Gaming.Unity;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Unity;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.UnityPackage
{
    /// <summary>
    /// <see cref="ILocalStorage"/> extensions for saving and loading data without dependencies.
    /// </summary>
    public static class DataStorageExtensions
    {
        /// <summary>
        /// Load data one time.
        /// </summary>
        /// <param name="storable">Storable to be loaded.</param>
        /// <typeparam name="TStorable">Type of storable to be loaded.</typeparam>
        /// <returns>Loaded storable.</returns>
        public static Task<TStorable> LoadOneTime<TStorable>(this TStorable storable)
            where TStorable : class, IStorable
        {
            return storable.LoadOneTime(DataStorageDependencies(storable));
        }

        /// <summary>
        /// Save data one time.
        /// </summary>
        /// <param name="storable">Storable to be saved.</param>
        /// <typeparam name="TStorable">Type of storable to be saved.</typeparam>
        /// <returns>Awaitable task.</returns>
        public static Task SaveOneTime<TStorable>(this TStorable storable)
            where TStorable : class, IStorable
        {
            return storable.SaveOneTime(DataStorageDependencies(storable));
        }

        /// <summary>
        /// Clear data one time.
        /// </summary>
        /// <param name="storable">Storable to be cleared.</param>
        /// <typeparam name="TStorable">Type of storable to be cleared.</typeparam>
        public static void ClearOneTime<TStorable>(this TStorable storable)
            where TStorable : class, IStorable
        {
            storable.ClearOneTime(DataStorageDependencies(storable));
        }

        private static IServiceCollection DataStorageDependencies<TStorable>(TStorable storable)
            where TStorable : class, IStorable
        {
            return new ServiceCollection()
#if UNITY_WEBGL && !UNITY_EDITOR
                .AddSingleton<ILocalStorage, WebDataStorage>()
#else
                .AddSingleton<ILocalStorage, DataStorage>()
#endif
                .AddSingleton<IStorable>(storable)
                .AddSingleton<ILogWriter, UnityLogWriter>()
                .AddSingleton<IOperatingSystemMediator, UnityOperatingSystemMediator>()
                .AddSingleton<IMainThreadRunner, UnityDispatcherAdapter>();
        }
    }
}
