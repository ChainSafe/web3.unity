using UnityEngine;

namespace MetaMask.IO
{
    public static class MetaMaskUnityStorage
    {
        private static IMetaMaskPersistentStorage _instance;

        public static IMetaMaskPersistentStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    BuildPlatformStorage();
                }

                return _instance;
            }
        }

        private static void BuildPlatformStorage()
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            _instance = MetaMaskWebLocalStorage.Singleton;
            #else
            _instance = MetaMaskPlayerPrefsStorage.Singleton;
            #endif
        }
    }
}