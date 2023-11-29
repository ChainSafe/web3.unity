#if UNITY_WEBGL
using System.Runtime.InteropServices;
#else
using System;
using UnityEngine;
#endif

namespace MetaMask.IO
{
    public class MetaMaskWebLocalStorage : IMetaMaskPersistentStorage
    {
        /// <summary>The singleton instance of the class.</summary>
        protected static MetaMaskWebLocalStorage instance;

        /// <summary>Gets the singleton instance of the class.</summary>
        /// <returns>The singleton instance of the class.</returns>
        public static MetaMaskWebLocalStorage Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new MetaMaskWebLocalStorage();
                }

                return instance;
            }
        }
        
#if UNITY_WEBGL
        [DllImport("__Internal")]
        public static extern bool LSExists(string key);

        [DllImport("__Internal")]
        public static extern void LSWrite(string key, string data);

        [DllImport("__Internal")]
        public static extern string LSRead(string key);

        [DllImport("__Internal")]
        public static extern void LSDelete(string key);
#else
        public static bool LSExists(string key)
        {
            throw new NotImplementedException("Incorrect platform, expected WebGL");
        }

        public static void LSWrite(string key, string data)
        {
            throw new NotImplementedException("Incorrect platform, expected WebGL");
        }

        public static string LSRead(string key)
        {
            throw new NotImplementedException("Incorrect platform, expected WebGL");
        }

        public static void LSDelete(string key)
        {
            throw new NotImplementedException("Incorrect platform, expected WebGL");
        }
#endif

        /// <summary>Creates a new instance of the <see cref="MetaMaskPlayerPrefsStorage"/> class.</summary>
        protected MetaMaskWebLocalStorage() { }

        /// <summary>Determines whether a key exists in the PlayerPrefs database.</summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether the key exists in the PlayerPrefs database.</returns>
        public bool Exists(string key)
        {
            return LSExists(key);
        }

        /// <summary>Writes a string to the persistent storage.</summary>
        /// <param name="key">The key to write to.</param>
        /// <param name="data">The data to write.</param>
        public void Write(string key, string data)
        {
            LSWrite(key, data);
        }

        /// <summary>Reads a string from persistent storage.</summary>
        /// <param name="key">The key to write to.</param>
        public string Read(string key)
        {
            return LSRead(key);
        }

        public void Delete(string key)
        {
            LSDelete(key);
        }
    }
}