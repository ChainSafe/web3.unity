using System;
using System.IO;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;
using WalletConnectSharp.Common.Logging;
using WalletConnectSharp.Storage;

namespace ChainSafe.Gaming.WalletConnect.Storage
{
    public class DataStorage : IDataStorage
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

        public async Task<LocalData> LoadLocalData()
        {
            var path = BuildLocalDataPath();
            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                var json = await File.ReadAllTextAsync(path); // todo use sync for WebGL?
                return JsonConvert.DeserializeObject<LocalData>(json);
            }
            catch
            {
                logWriter.LogError("WalletConnect local data file is corrupted. Removing..");
                File.Delete(path);
                return null;
            }
        }

        public async Task SaveLocalData(LocalData localData)
        {
            var path = BuildLocalDataPath();
            var json = JsonConvert.SerializeObject(localData);
            await File.WriteAllTextAsync(path, json); // todo use sync for WebGL?
        }

        public void ClearLocalData()
        {
            var path = BuildLocalDataPath();
            if (!File.Exists(path))
            {
                return;
            }

            File.Delete(path);
        }

        public FileSystemStorage BuildStorage(bool sessionStored)
        {
            var path = Path.Combine(osMediator.ApplicationDataPath, config.StoragePath, StorageFileName);

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
            return Path.Combine(osMediator.ApplicationDataPath, config.StoragePath, LocalDataFileName);
        }
    }
}