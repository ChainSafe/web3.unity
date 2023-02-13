using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
// add this back in for production to initialize pop up and registration check
[InitializeOnLoad]
public class Startup : EditorWindow
{
    static Startup()
    {
        if (PlayerPrefs.GetString("Registered") == "")
        {
            Debug.LogError("ProjectID Not Valid! Please Go To Dashboard.Gaming.Chainsafe.io To Get A New ProjectID");
            EditorApplication.ExecuteMenuItem("Window/ChainSafeServerSettings");
            // Get existing open window or if none, make a new one:
        }
    }
}
#endif