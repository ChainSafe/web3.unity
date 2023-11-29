using MetaMask.SocketIOClient;
using MetaMask.Unity;
using UnityEngine;

namespace MetaMask.IO
{
    public class MetaMaskPlayerPrefsStorage : IMetaMaskPersistentStorage
    {
        /// <summary>The singleton instance of the class.</summary>
        protected static MetaMaskPlayerPrefsStorage instance;

        /// <summary>Gets the singleton instance of the class.</summary>
        /// <returns>The singleton instance of the class.</returns>
        public static MetaMaskPlayerPrefsStorage Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new MetaMaskPlayerPrefsStorage();
                }

                return instance;
            }
        }

        /// <summary>Creates a new instance of the <see cref="MetaMaskPlayerPrefsStorage"/> class.</summary>
        protected MetaMaskPlayerPrefsStorage() { }

        /// <summary>Determines whether a key exists in the PlayerPrefs database.</summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether the key exists in the PlayerPrefs database.</returns>
        public bool Exists(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>Writes a string to the persistent storage.</summary>
        /// <param name="key">The key to write to.</param>
        /// <param name="data">The data to write.</param>
        public void Write(string key, string data)
        {
            if (MetaMaskUnity.Instance.IsInUnityThread())
            {
                DoWrite(key, data);
            }
            else
            {
                UnityThread.executeInUpdate(() => { DoWrite(key, data); });
            }
        }

        /// <summary>Reads a string from persistent storage.</summary>
        /// <param name="key">The key to write to.</param>
        public string Read(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void Delete(string key)
        {
            if (MetaMaskUnity.Instance.IsInUnityThread())
            {
                DoDelete(key);
            }
            else
            {
                UnityThread.executeInUpdate(() => { DoDelete(key); });
            }
        }

        private void DoWrite(string key, string data)
        {
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }

        private void DoDelete(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }
    }
}