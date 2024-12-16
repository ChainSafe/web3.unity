using UnityEditor;
using UnityEngine;

namespace ChainSafe.GamingSdk.Editor
{
    public static class Web3MenuItems
    {
        private const string PrefabGUID = "6d183a51c0030794a8ff3fea5dbd6423";
        private const string GameObjectName = "Web3 Client";

        [MenuItem("GameObject/Web3/Web3 Client", false, 10)]
        public static void CreateWeb3UnityClient(MenuCommand menuCommand)
        {
            var prefabPath = AssetDatabase.GUIDToAssetPath(PrefabGUID);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            var client = Object.Instantiate(prefab);
            client.name = GameObjectName;
            
            GameObjectUtility.SetParentAndAlign(client, menuCommand.context as GameObject); // set parent to spawn object in the selected scene
            client.transform.SetParent(null); // reset parent as client will be marked DontDestroyOnLoad in runtime
            
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(client, "Create " + client.name);
            Selection.activeObject = client;
        }
    }
}