using System;
using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;
using Reown.Core.Storage;
using Reown.Core.Storage.Interfaces;

namespace ChainSafe.Gaming.Reown.Storage
{
    public class ReownStorageFactory
    {
        private const string RelativeFilePath = "Reown/storage.json";

        public static async Task<IKeyValueStorage> Build(Web3Environment environment)
        {
            switch (environment.OperatingSystem.Platform)
            {
                case Platform.Editor:
                case Platform.Android:
                case Platform.IOS:
                case Platform.Desktop:
                    return await BuildFileSystemStorage(environment);
                case Platform.WebGL:
                    return BuildWebGlStorage();
                default:
                    throw new ReownIntegrationException($"{environment.OperatingSystem.Platform} is not supported.");
            }
        }

        private static async Task<FileSystemStorage> BuildFileSystemStorage(Web3Environment environment)
        {
            var storageFilePath = Path.Combine(environment.OperatingSystem.AppPersistentDataPath, RelativeFilePath);
            var storage = new FileSystemStorage(storageFilePath);

            try
            {
                await storage.Init();
            }
            catch (JsonException)
            {
                environment.LogWriter.LogError($"Failed to deserialize storage. Deleting it and creating a new one at <i>{storageFilePath}</i>");
                await storage.Clear();
                await storage.Init();
            }

            return storage;
        }

        private static IKeyValueStorage BuildWebGlStorage()
        {
            throw new NotImplementedException();
        }
    }
}