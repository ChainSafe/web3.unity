using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.LocalStorage
{
    /// <summary>
    /// Default implementation of the component responsible for storage of persistent data.
    /// </summary>
    public class DataStorage : ILocalStorage
    {
        private readonly IEnumerable<IStorable> store;
        private readonly IOperatingSystemMediator osMediator;
        private readonly ILogWriter logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStorage"/> class.
        /// </summary>
        /// <param name="store">All injected <see cref="IStorable"/>.</param>
        /// <param name="osMediator">Injected <see cref="IOperatingSystemMediator"/>.</param>
        /// <param name="logWriter">Injected <see cref="ILogWriter"/>.</param>
        public DataStorage(IEnumerable<IStorable> store, IOperatingSystemMediator osMediator, ILogWriter logWriter)
        {
            this.store = store;
            this.osMediator = osMediator;
            this.logWriter = logWriter;
        }

        /// <summary>
        /// Initialize all storable data.
        /// </summary>
        /// <returns>Awaitable task for Initialize operation.</returns>
        public async Task Initialize()
        {
            foreach (var storable in store)
            {
                if (storable.LoadOnInitialize && Exists(AbsolutePath(storable.StoragePath)))
                {
                    await Load(storable);
                }
            }
        }

        /// <summary>
        /// Save storable data to local storage.
        /// </summary>
        /// <param name="storable">Storable data to be saved.</param>
        /// <param name="createFile">Create new file if file doesn't exist.</param>
        /// <typeparam name="T">Type of Storable to be saved. Helps for serializing.</typeparam>
        /// <returns>Awaitable Task for save operation.</returns>
        public virtual async Task Save<T>(T storable, bool createFile = true)
            where T : IStorable
        {
            var path = AbsolutePath(storable.StoragePath);

            if (!createFile && !Exists(path))
            {
                return;
            }

            try
            {
                var json = JsonConvert.SerializeObject(storable);

                await File.WriteAllTextAsync(path, json);
            }
            catch (Exception e)
            {
                logWriter.LogError($"Failed to save {storable.StoragePath} : {e.Message} : {e}");
            }
        }

        /// <summary>
        /// Load storable data from local storage.
        /// </summary>
        /// <param name="storable">Storable data to be loaded.</param>
        /// <typeparam name="T">Type of Storable to be loaded. Helps for deserializing.</typeparam>
        /// <returns>Awaitable Task for load operation.</returns>
        public async Task Load<T>(T storable)
            where T : IStorable
        {
            var path = AbsolutePath(storable.StoragePath);

            if (!Exists(path))
            {
                logWriter.Log($"Failed to load {storable.StoragePath} : File not found.");

                return;
            }

            try
            {
                var json = await File.ReadAllTextAsync(path);

                JsonConvert.PopulateObject(json, storable);
            }
            catch
            {
                logWriter.LogError($"Local data file for {storable.StoragePath} is corrupted. Removing..");

                File.Delete(path);
            }
        }

        /// <summary>
        /// Clear storable data from local storage.
        /// </summary>
        /// <param name="storable">Storable data to be cleared.</param>
        public void Clear(IStorable storable)
        {
            string path = AbsolutePath(storable.StoragePath);

            if (!Exists(path))
            {
                logWriter.LogError($"Failed to clear {storable.StoragePath} : File not found.");

                return;
            }

            File.Delete(path);
        }

        private bool Exists(string path)
        {
            return File.Exists(path);
        }

        private string AbsolutePath(string path)
        {
            return Path.Combine(osMediator.AppPersistentDataPath, path);
        }
    }
}