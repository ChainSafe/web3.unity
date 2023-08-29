using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectConfigUtilities
{
    public static ProjectConfigScriptableObject Load()
    {
        var projectConfig = Resources.Load<ProjectConfigScriptableObject>("ProjectConfigData");
        return projectConfig ? projectConfig : null;
    }

#if UNITY_EDITOR
    public static ProjectConfigScriptableObject CreateOrLoad()
    {
        var projectConfig = Load();
        if (projectConfig == null)
        {
            projectConfig = ScriptableObject.CreateInstance<ProjectConfigScriptableObject>();
            UnityEditor.AssetDatabase.CreateAsset(projectConfig, "Assets/Resources/ProjectConfigData.asset");
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
