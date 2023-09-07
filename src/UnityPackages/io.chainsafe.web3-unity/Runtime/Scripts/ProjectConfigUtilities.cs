using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ProjectConfigUtilities
{
    private const string AssetName = "ProjectConfigData";

    public static ProjectConfigScriptableObject Load()
    {
        var projectConfig = Resources.Load<ProjectConfigScriptableObject>(AssetName);
        return projectConfig ? projectConfig : null;
    }

#if UNITY_EDITOR
    public static ProjectConfigScriptableObject CreateOrLoad()
    {
        var projectConfig = Load();

        if (projectConfig == null)
        {
            string assetDirectory = Path.Combine(Application.dataPath, nameof(Resources));

            if (!Directory.Exists(assetDirectory))
            {
                Directory.CreateDirectory(assetDirectory);
            }

            projectConfig = ScriptableObject.CreateInstance<ProjectConfigScriptableObject>();
            UnityEditor.AssetDatabase.CreateAsset(projectConfig,
                Path.Combine("Assets", nameof(Resources), $"{AssetName}.asset"));
        }

        return projectConfig;
    }

    public static void Save(ProjectConfigScriptableObject projectConfig)
    {
        UnityEditor.EditorUtility.SetDirty(projectConfig);
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}