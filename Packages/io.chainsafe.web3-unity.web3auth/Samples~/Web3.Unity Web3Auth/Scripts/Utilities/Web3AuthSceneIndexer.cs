#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Add Web3.Unity Web3Auth package sample scenes to Build Settings Scenes 
/// </summary>
[InitializeOnLoad]
public static class Web3AuthSceneIndexer
{
    private const string ScenesIndexedKey = PackageName + "ScenesIndexed";

    private const string PackageName = "io.chainsafe.web3-unity.web3auth";
    
    static Web3AuthSceneIndexer()
    {
        SceneIndexer.TryAddEditorBuildSettingsScenes(PackageName, ScenesIndexedKey, new string[]
        {
            "SampleLogin - Web3Auth.Unity",
        });
    }
}

#endif