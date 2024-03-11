using System;
using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Storage;

namespace ChainSafe.Gaming.WalletConnect.Storage
{
    /// <summary>
    /// Default implementation of the component responsible for storage of WalletConnect-related data.
    /// </summary>
    public class DataStorage
    {
        private const string StorageFileName = "walletconnect-storage.json";
        private const string LocalDataFileName = "walletconnect-local.json";

        private readonly IWalletConnectConfig config;
        private readonly IOperatingSystemMediator osMediator;
        private readonly ILogWriter logWriter;

        public DataStorage(IWalletConnectConfig config, IOperatingSystemMediator osMediator, ILogWriter logWriter)
        {
            this.logWriter = logWriter;
            this.osMediator = osMediator;
            this.config = config;
        }

        /// <summary>
        /// Loads local data for the WalletConnect integration.
        /// </summary>
        /// <returns>Local data for the WalletConnect integration.</returns>
        public async Task<LocalData> LoadLocalData()
        {
            var path = BuildLocalDataPath();
            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                var json = await File.ReadAllTextAsync(path);
                return JsonConvert.DeserializeObject<LocalData>(json);
            }
            catch
            {
                logWriter.LogError("WalletConnect local data file is corrupted. Removing..");
                File.Delete(path);
                return null;
            }
        }

        /// <summary>
        /// Saves local data for the WalletConnect integration.
        /// </summary>
        /// <param name="localData">The data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SaveLocalData(LocalData localData)
        {
            var path = BuildLocalDataPath();
            var json = JsonConvert.SerializeObject(localData);
            await File.WriteAllTextAsync(path, json);
        }

        /// <summary>
        /// Clears local data for the WalletConnect integration  from the disk.
        /// </summary>
        public void ClearLocalData()
        {
            var path = BuildLocalDataPath();
            if (!File.Exists(path))
            {
                return;
            }

            File.Delete(path);
        }

        /// <summary>
        /// Builds storage for WalletConnectCSharp.
        /// </summary>
        /// <param name="sessionStored">True if the session was already stored.</param>
        /// <returns>Storage object used by WalletConnectCSharp.</returns>
        public FileSystemStorage BuildStorage(bool sessionStored)
        {
            var absStoragePath = BuildStoragePath(osMediator.AppPersistentDataPath, config.StoragePath);
            var path = Path.Combine(absStoragePath, StorageFileName);

            // If we're not restoring a session and save WC file exists remove it.
            // This is done to mitigate for a WC error that happens intermittently where generated Uri doesn't connect wallet.
            if (!sessionStored && File.Exists(path))
            {
                File.Delete(path);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            WCLogger.Log($"Wallet Connect Storage set to {path}");

            return new FileSystemStorage(path);
        }

        private string BuildLocalDataPath()
        {
            var absStoragePath = BuildStoragePath(osMediator.AppPersistentDataPath, config.StoragePath);
            return Path.Combine(absStoragePath, LocalDataFileName);
        }

        public static string BuildStoragePath(string appDataPath, string storageRelativePath)
        {
            return Path.Combine(appDataPath, storageRelativePath);
        }
    }
}