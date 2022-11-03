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
            Debug.LogError("ProjectID Not Valid! Please Go To Dashboard.Gaming.Chainsafe.io To Get A New ProjectID");
        }
    }
}
#endif 