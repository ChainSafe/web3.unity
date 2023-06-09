using System.IO;
using UnityEditor;
using UnityEngine;

internal class PluginPathDetector : ScriptableObject
{
    internal static string GetPluginPath()
    {
        var ms = MonoScript.FromScriptableObject(CreateInstance<PluginPathDetector>());
        var filePath = AssetDatabase.GetAssetPath(ms);

        var directory = new FileInfo(filePath).Directory;
        if (directory.Name != "Editor")
        {
            throw new System.Exception($"Expected {nameof(PluginPathDetector)} to be in an 'Editor' directory");
        }

        return directory.Parent.FullName.Replace('\\', '/');
    }
}
