using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace ChainSafe.Gaming.LocalStorage
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
        /// <param name="services">Service collection with <see cref="ILocalStorage"/> dependecies.</param>
        /// <typeparam name="TStorable">Type of storable to be loaded.</typeparam>
        /// <returns>Loaded storable.</returns>
        public static async Task<TStorable> LoadOneTime<TStorable>(this TStorable storable, IServiceCollection services)
            where TStorable : class, IStorable
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var localStorage = serviceProvider.GetService<ILocalStorage>();

            await localStorage.Load(storable);

            return storable;
        }

        /// <summary>
        /// Save data one time.
        /// </summary>
        /// <param name="storable">Storable to be saved.</param>
        /// <param name="services">Service collection with <see cref="ILocalStorage"/> dependecies.</param>
        /// <typeparam name="TStorable">Type of storable to be saved.</typeparam>
        /// <returns>Awaitable task.</returns>
        public static async Task SaveOneTime<TStorable>(this TStorable storable, IServiceCollection services)
            where TStorable : class, IStorable
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var localStorage = serviceProvider.GetService<ILocalStorage>();

            await localStorage.Save(storable);
        }

        /// <summary>
        /// Clear data one time.
        /// </summary>
        /// <param name="storable">Storable to be cleared.</param>
        /// <param name="services">Service collection with <see cref="ILocalStorage"/> dependecies.</param>
        /// <typeparam name="TStorable">Type of storable to be cleared.</typeparam>
        public static void ClearOneTime<TStorable>(this TStorable storable, IServiceCollection services)
            where TStorable : class, IStorable
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var dataStorage = serviceProvider.GetService<ILocalStorage>();

            dataStorage.Clear(storable);
        }
    }
}