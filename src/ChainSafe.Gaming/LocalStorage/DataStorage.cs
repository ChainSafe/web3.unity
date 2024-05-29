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
    /// Default implementation of the component responsible for storage of WalletConnect-related data.
    /// </summary>
    public class DataStorage
    {
        private readonly IOperatingSystemMediator osMediator;
        private readonly ILogWriter logWriter;

        public DataStorage(IOperatingSystemMediator osMediator, ILogWriter logWriter)
        {
            this.osMediator = osMediator;
            this.logWriter = logWriter;
        }

        public async Task Save<T>(T storable, bool createFile = true)
            where T : IStorable
        {
            var path = AbsolutePath(storable.StoragePath);

            if (!createFile && !Exists(path))
            {
                return;
            }

            // try catch.
            var json = JsonConvert.SerializeObject(storable);
            logWriter.Log($"Saved {json}");
            await File.WriteAllTextAsync(path, json);
        }

        public async Task Load<T>(T storable)
            where T : IStorable
        {
            var path = AbsolutePath(storable.StoragePath);

            if (!Exists(path))
            {
                return;
            }

            try
            {
                var json = await File.ReadAllTextAsync(path);

                logWriter.Log($"Loaded {json}");

                JsonConvert.PopulateObject(json, storable);
            }
            catch
            {
                logWriter.LogError("WalletConnect local data file is corrupted. Removing..");

                File.Delete(path);
            }
        }

        public void Clear<T>(T storable)
            where T : IStorable
        {
            string path = AbsolutePath(storable.StoragePath);

            if (!Exists(path))
            {
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