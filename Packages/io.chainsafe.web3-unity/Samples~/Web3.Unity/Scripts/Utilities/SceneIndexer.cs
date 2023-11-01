#if UNITY_EDITOR

using System;
using System.IO;
using System.Linq;
using Scenes;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

[InitializeOnLoad]
public static class SceneIndexer
{
    private const string ScenesIndexedKey = PackageName + "ScenesIndexed";

    private const string PackageName = "io.chainsafe.web3-unity";

    static SceneIndexer()
    {
        TryAddEditorBuildSettingsScenes(PackageName, ScenesIndexedKey, new string[]
        {
            "SampleLogin.Unity",
            $"{Login.MainSceneName}.Unity",
            "SampleImportNftTexture.Unity",
        });
    }

    public static void TryAddEditorBuildSettingsScenes(string packageName, string sessionKey, string[] scenes)
    {
        if (SessionState.GetBool(sessionKey, false))
        {
            return;
        }

        PackageInfo package = GetPackage(packageName);

        string importPath = GetImportPath(package);

        //scenes already added to build settings
        if (EditorBuildSettings.scenes.Any(s => Path.GetFullPath(s.path).Contains(importPath)))
        {
            SessionState.SetBool(sessionKey, true);

            return;
        }

        // convert and add importPath to scenes path
        EditorBuildSettingsScene[] editorBuildSettingsScenes = Array.ConvertAll(scenes, s => new EditorBuildSettingsScene(Path.Combine(importPath, s), true));
        
        EditorBuildSettings.scenes = editorBuildSettingsScenes.Concat(EditorBuildSettings.scenes).ToArray();
        
        SessionState.SetBool(sessionKey, true);
    }
    
    private static PackageInfo GetPackage(string name)
    {
        var listRequest = Client.List();

        while (!listRequest.IsCompleted)
        {
            //do nothing
        }

        PackageInfo[] packages = listRequest.Result.ToArray();

        PackageInfo package = packages.FirstOrDefault(p => p.name == name);

        if (package == null)
        {
            throw new Exception($"Installed Package {name} not found");
        }
        
        return package;
    }

    private static string GetImportPath(PackageInfo package)
    {
        Sample sample = Sample.FindByPackage(package.name, package.version).FirstOrDefault();

        string importPath = Path.GetRelativePath(Directory.GetCurrentDirectory(), sample.importPath);

        return Path.Combine(importPath, "Scenes");
    }
}

#endif