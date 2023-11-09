
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

[InitializeOnLoad]
public static class PreBuild
{
    static PreBuild()
    {
        if (SessionState.GetBool(nameof(PreBuild), false))
        {
            return;
        }
        
        Debug.Log("Running Pre Build Operations...");
        
#if UNITY_IOS
        PlayerSettings.stripEngineCode = false;
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Disabled);
        
        Debug.Log("Code managed stripping Level Disabled for IOS");
#endif

        SessionState.SetBool(nameof(PreBuild), true);
    }
}