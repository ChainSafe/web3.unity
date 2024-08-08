using System.IO;
using System.Threading.Tasks;

namespace ChainSafe.Gaming.LocalStorage
{
    /// <summary>
    /// Local storage used for saving and loaded persistent data locally.
    /// </summary>
    public interface ILocalStorage
    {
        /// <summary>
        /// Initialized all storable data here.
        /// </summary>
        /// <returns>Awaitable Task.</returns>
        Task Initialize();

        /// <summary>
        /// Save storable data to local storage.
        /// </summary>
        /// <param name="storable">Storable data to be saved.</param>
        /// <param name="createFile">Create storable file if it doesn't exit.</param>
        /// <typeparam name="T">Type of storable data to be saved.</typeparam>
        /// <returns>Awaitable Task.</returns>
        Task Save<T>(T storable, bool createFile = true)
            where T : IStorable;

        /// <summary>
        /// Load storable data from local storage.
        /// </summary>
        /// <param name="storable">Storable data to be loaded.</param>
        /// <typeparam name="T">Type of storable data to be loaded.</typeparam>
        /// <returns>Awaitable Task.</returns>
        Task Load<T>(T storable)
            where T : IStorable;

        /// <summary>
        /// Clear/Delete storable data for local storage.
        /// </summary>
        /// <param name="storable">Storable data to be cleared.</param>
        void Clear(IStorable storable);
    }
}