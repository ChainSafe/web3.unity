using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reown.Core.Common.Logging;
using Reown.Core.Storage;
using UnityEngine;

namespace Reown.Sign.Unity
{
    public class PlayerPrefsStorage : InMemoryStorage
    {
        private const string StorageKey = "Reown.Sign.Unity.PlayerPrefsStorage";
        private readonly JsonSerializerSettings _jsonSerializerLoadSettings;
        private readonly JsonSerializerSettings _jsonSerializerSaveSettings;

        private readonly SynchronizationContext _mainThreadContext;
        private SemaphoreSlim _semaphoreSlim;

        public PlayerPrefsStorage(SynchronizationContext mainThreadContext)
        {
            _mainThreadContext = mainThreadContext;
            _jsonSerializerSaveSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            _jsonSerializerLoadSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        public override async Task Init()
        {
            _semaphoreSlim = new SemaphoreSlim(1, 1);
            await Task.WhenAll(
                Load(), base.Init()
            );
        }

        public override async Task SetItem<T>(string key, T value)
        {
            await base.SetItem(key, value);
            await Save();
        }

        public override async Task RemoveItem(string key)
        {
            await base.RemoveItem(key);
            await Save();
        }

        public override async Task Clear()
        {
            await base.Clear();
            await Save();
        }

        private async Task Load()
        {
            await _semaphoreSlim.WaitAsync();
            var tcs = new TaskCompletionSource<bool>();

            _mainThreadContext.Post(_ =>
                {
                    if (PlayerPrefs.HasKey(StorageKey))
                    {
                        var json = PlayerPrefs.GetString(StorageKey);
                        try
                        {
                            Entries = JsonConvert.DeserializeObject<ConcurrentDictionary<string, object>>(
                                json,
                                _jsonSerializerLoadSettings
                            );
                        }
                        catch (JsonSerializationException e)
                        {
                            ReownLogger.LogError(e);
                            ReownLogger.LogError("Cannot load JSON from PlayerPrefs, starting fresh");
                            Entries = new ConcurrentDictionary<string, object>();
                        }
                    }

                    tcs.SetResult(true);
                },
                null
            );

            await tcs.Task;
            _semaphoreSlim.Release();
        }

        private async Task Save()
        {
            var json = JsonConvert.SerializeObject(Entries, _jsonSerializerSaveSettings);

            await _semaphoreSlim.WaitAsync();

            _mainThreadContext.Post(_ =>
            {
                PlayerPrefs.SetString(StorageKey, json);
                PlayerPrefs.Save();
            }, null);

            _semaphoreSlim.Release();
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                _semaphoreSlim.Dispose();
            }

            Disposed = true;
        }
    }
}