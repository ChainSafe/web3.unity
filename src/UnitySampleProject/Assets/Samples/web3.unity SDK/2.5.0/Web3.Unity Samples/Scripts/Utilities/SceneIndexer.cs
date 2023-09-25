#if UNITY_EDITOR

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

[InitializeOnLoad]
public static class SceneIndexer
{
    private const string ScenesIndexedKey = "ScenesIndexed";

    private const string PackageName = "io.chainsafe.web3-unity";

    static SceneIndexer()
    {
        if (SessionState.GetBool(ScenesIndexedKey, false))
        {
            return;
        }

        var listRequest = Client.List();

        while (!listRequest.IsCompleted)
        {
            //do nothing
        }

        PackageInfo[] packages = listRequest.Result.ToArray();

        PackageInfo package = packages.FirstOrDefault(p => p.name == PackageName);

        if (package == null)
        {
            Debug.LogError($"Installed Package {PackageName} not found");

            return;
        }

        Sample sample = Sample.FindByPackage(package.name, package.version).FirstOrDefault();

        string importPath = Path.GetRelativePath(Directory.GetCurrentDirectory(), sample.importPath);

        importPath = Path.Combine(importPath, "Scenes");

        //scenes already added to build settings
        if (EditorBuildSettings.scenes.Any(s => s.path.Contains(importPath)))
        {
            SessionState.SetBool(ScenesIndexedKey, true);

            return;
        }

        EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene(Path.Combine(importPath, "SampleLogin.Unity"), true),
                new EditorBuildSettingsScene(Path.Combine(importPath, "SampleMain.Unity"), true),
                new EditorBuildSettingsScene(Path.Combine(importPath, "SampleImportNftTexture.Unity"), true),
            }.Concat(EditorBuildSettings.scenes)
            .ToArray();

        SessionState.SetBool(ScenesIndexedKey, true);
    }
}

#endif