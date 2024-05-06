using System.IO;
using ChainSafe.Gaming.WalletConnect.Storage;
using ChainSafe.Gaming.WalletConnect.Wallets;
using UnityEditor;
using UnityEngine;

namespace ChainSafe.Gaming.WalletConnect.Editor
{
    [CustomEditor(typeof(WalletConnectConfigSO))]
    public class WalletConnectConfigSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("List Wallet Providers", GUILayout.ExpandWidth(false)))
                {
                    ListWalletProviders();
                }

                if (GUILayout.Button("Clear cache", GUILayout.ExpandWidth(false)))
                {
                    DeleteStorage();
                }
            }
        }

        private void ListWalletProviders()
        {
            var config = (WalletConnectConfigSO)target;

            if (string.IsNullOrWhiteSpace(config.ProjectId))
            {
                Debug.LogError("Project Id required.");
                return;
            }

            var uri = WalletRegistry.BuildRegistryUri(config.ProjectId);
            Application.OpenURL(uri);
        }

        private void DeleteStorage()
        {
            var config = (WalletConnectConfigSO)target;

            if (string.IsNullOrEmpty(config.StoragePath))
            {
                Debug.LogError("StoragePath is empty.");
                return;
            }

            var storageFolderPath =
                DataStorage.BuildStoragePath(Application.persistentDataPath, config.StoragePath);

            if (!Directory.Exists(storageFolderPath))
            {
                Debug.Log("WalletConnect cache is already cleared.");
                return;
            }

            Directory.Delete(storageFolderPath, true);

            Debug.Log("WalletConnect cache cleared.");
        }
    }
}