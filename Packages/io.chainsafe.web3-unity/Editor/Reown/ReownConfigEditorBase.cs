using ChainSafe.Gaming.Reown.Wallets;
using UnityEditor;
using UnityEngine;

namespace ChainSafe.Gaming.Editor.Reown
{
    public class ReownConfigEditorBase : UnityEditor.Editor
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

                // if (GUILayout.Button("Clear cache", GUILayout.ExpandWidth(false)))
                // {
                //     DeleteStorage();
                // }
            }
        }

        private void ListWalletProviders()
        {
            Application.OpenURL(ReownWalletRegistry.RegistryUri);
        }

        // private void DeleteStorage() // todo check if this is needed, remove otherwise
        // {
        //     var config = (IReownConfig)target;
        //
        //     if (string.IsNullOrEmpty(config.StoragePath))
        //     {
        //         Debug.LogError("StoragePath is empty.");
        //         return;
        //     }
        //
        //     var storageFolderPath =
        //         DataStorage.BuildStoragePath(Application.persistentDataPath, config.StoragePath);
        //
        //     if (!Directory.Exists(storageFolderPath))
        //     {
        //         Debug.Log("Reown cache is already cleared.");
        //         return;
        //     }
        //
        //     Directory.Delete(storageFolderPath, true);
        //
        //     Debug.Log("Reown cache cleared.");
        // }
    }
}