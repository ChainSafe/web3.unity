using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public class Startup
{
    static Startup()
    {
        if (PlayerPrefs.GetString("Registered") == "")
        {
            EditorApplication.ExecuteMenuItem("Window/ChainSafeServerSettings");
            Debug.LogError("ProjectID Not Valid! Please Go To Dashboard.Gaming.Chainsafe.io To Get A New ProjectID & Enter It By Pressing Window -> ChainsafeServerSettings");
        }
    }
}
#endif 
