#if UNITY_WEBGL && !UNITY_EDITOR
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChainSafe.Gaming.LocalStorage;
using Newtonsoft.Json;
using UnityEngine;

namespace ChainSafe.Gaming.UnityPackage
{
    /// <summary>
    /// Save and Load persistent data on WebGL.
    /// </summary>
    public class WebDataStorage : ILocalStorage
    {
        [DllImport("__Internal")]
        private static extern void Save(string key, string value);
        
        [DllImport("__Internal")]
        private static extern string Load(string key);
        
        [DllImport("__Internal")]
        private static extern void Clear(string key);


        private readonly IEnumerable<IStorable> _store;

        public WebDataStorage(IEnumerable<IStorable> store)
        {
            _store = store;
        }
        
        public async Task Initialize()
        {
            foreach (var storable in _store)
            {
                if (storable.LoadOnInitialize)
                {
                    await Load(storable);
                }
            }
        }

        public Task Save<T>(T storable, bool createFile = true) where T : IStorable
        {
            string json = JsonConvert.SerializeObject(storable);

            Save(storable.StoragePath, json);

            return Task.CompletedTask;
        }

        public Task Load<T>(T storable) where T : IStorable
        {
            string json = Load(storable.StoragePath);

            if (string.IsNullOrEmpty(json) || json == JsonConvert.Null)
            {
                Debug.Log($"Failed to load {storable.StoragePath} : File not found.");
                
                return Task.CompletedTask;
            }

            json = Regex.Unescape(json.Trim('"'));

            JsonConvert.PopulateObject(json, storable);

            return Task.CompletedTask;
        }

        public void Clear(IStorable storable)
        {
            Clear(storable.StoragePath);
        }
    }
}
#endif