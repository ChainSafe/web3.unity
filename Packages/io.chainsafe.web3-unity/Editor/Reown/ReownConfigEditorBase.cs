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
            }
        }

        private void ListWalletProviders()
        {
            Application.OpenURL(ReownWalletRegistry.RegistryUri);
        }
    }
}