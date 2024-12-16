#if UNITY_WEBGL
using ChainSafe.Gaming.UnityPackage.Connection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ChainSafe.Gaming.Editor.Reown
{
    public class ReownWebGLPreBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            // Use AssetDatabase to load the TextAsset from Editor/Resources
            string assetPath = "Packages/io.chainsafe.web3-unity/Editor/Resources/ViemChain.txt";
            TextAsset file = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);

            if (file != null)
            {
                Debug.Log($"Loaded ViemChain.txt: {file.text}");
                // Call your custom method
                ReownConnectionProvider provider = Resources.Load<ReownConnectionProvider>("ReownConnectionProvider");
                provider.PopulateViemNames(file.text, true);
            }
            else
            {
                Debug.LogError($"Could not load ViemChain.txt from path: {assetPath}");
            }
        }

    }
}
#endif