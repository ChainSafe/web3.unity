using UnityEditor;

public class DllPlatformFixing : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        for (var i = 0; i < importedAssets.Length; i++)
            if (importedAssets[i].Contains("ChainSafe.Gaming.Unity.MetaMask"))
            {
                var importer = (PluginImporter)AssetImporter.GetAtPath(importedAssets[i]);
                if (importer == null) continue;
                //If it's already set to false, that means we've been here and can early return.
                if (!importer.GetCompatibleWithAnyPlatform())
                    return;

                importer.SetCompatibleWithAnyPlatform(false);
                importer.SetCompatibleWithPlatform(BuildTarget.WebGL, true);
                importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
                importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                importer.SetCompatibleWithEditor(false);


                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
    }
}