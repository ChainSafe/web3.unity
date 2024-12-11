using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reown.Core.Common.Logging;
using Reown.Core.Common.Model.Errors;
using Reown.Core.Crypto;
using Reown.Core.Storage;
using Reown.Core.Storage.Interfaces;
using Reown.Sign.Models;
using Reown.Sign.Models.Engine.Events;
using Reown.Sign.Unity.Utils;
using UnityEngine;

namespace Reown.Sign.Unity
{
    public class SignClientUnity : SignClient
    {
        public Linker Linker { get; }

        private bool _disposed;

        // --- Unity Events (Main Thread) ---
        public event EventHandler<SessionStruct> SessionConnectedUnity;
        public event EventHandler<SessionStruct> SessionUpdatedUnity;
        public event EventHandler SessionDisconnectedUnity;

        private SignClientUnity(SignClientOptions options) : base(options)
        {
            Linker = new Linker(this);

            SessionConnected += OnSessionConnected;
            SessionUpdateRequest += OnSessionUpdated;
            SessionDeleted += OnSessionDeleted;
        }

        public static async Task<SignClientUnity> Create(SignClientOptions options)
        {
            if (options.Storage == null)
            {
                var storage = await BuildUnityStorage();
                options.Storage = storage;
                options.KeyChain ??= new KeyChain(storage);
            }

            options.RelayUrlBuilder ??= new UnityRelayUrlBuilder();
            options.ConnectionBuilder ??= new ConnectionBuilderUnity();

            var sign = new SignClientUnity(options);
            await sign.Initialize();
            return sign;
        }

        private static async Task<IKeyValueStorage> BuildUnityStorage()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var currentSyncContext = System.Threading.SynchronizationContext.Current;
            if (currentSyncContext.GetType().FullName != "UnityEngine.UnitySynchronizationContext")
                throw new System.Exception(
                    $"[Reown.Sign.Unity] SynchronizationContext is not of type UnityEngine.UnitySynchronizationContext. Current type is <i>{currentSyncContext.GetType().FullName}</i>. When targeting WebGL, Make sure to initialize SignClient from the main thread.");

            var playerPrefsStorage = new PlayerPrefsStorage(currentSyncContext);
            await playerPrefsStorage.Init();

            return playerPrefsStorage;
#endif

            var path = $"{Application.persistentDataPath}/Reown/storage.json";
            ReownLogger.Log($"[Reown.Sign.Unity] Using storage path <i>{path}</i>");

            var storage = new FileSystemStorage(path);

            try
            {
                await storage.Init();
            }
            catch (JsonException)
            {
                Debug.LogError($"[Reown.Sign.Unity] Failed to deserialize storage. Deleting it and creating a new one at <i>{path}</i>");
                await storage.Clear();
                await storage.Init();
            }

            return storage;
        }

        public async Task<bool> TryResumeSessionAsync()
        {
            await AddressProvider.LoadDefaultsAsync();

            var sessionTopic = AddressProvider.DefaultSession.Topic;

            if (string.IsNullOrWhiteSpace(sessionTopic))
                return false;

            try
            {
                await Extend(sessionTopic);
            }
            catch (KeyNotFoundException)
            {
                AddressProvider.DefaultSession = default;
                return false;
            }
            catch (ReownNetworkException)
            {
                AddressProvider.DefaultSession = default;
                return false;
            }

            return true;
        }

        private void OnSessionConnected(object sender, SessionStruct session)
        {
            UnitySyncContext.Context.Post(_ => { SessionConnectedUnity?.Invoke(this, session); }, null);
        }

        private void OnSessionUpdated(object sender, SessionEvent sessionEvent)
        {
            var sessionStruct = Session.Values.First(s => s.Topic == sessionEvent.Topic);
            UnitySyncContext.Context.Post(_ => { SessionUpdatedUnity?.Invoke(this, sessionStruct); }, null);
        }

        private void OnSessionDeleted(object sender, SessionEvent _)
        {
            UnitySyncContext.Context.Post(_ => { SessionDisconnectedUnity?.Invoke(this, EventArgs.Empty); }, null);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Linker.Dispose();
            }

            base.Dispose(disposing);
            _disposed = true;
        }
    }
}