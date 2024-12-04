using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class CountlyEditorMenu
{
    [MenuItem("Countly/SDK Documentation")]
    private static void SDKDocumentation()
    {
        Application.OpenURL("https://support.count.ly/hc/en-us/articles/360037813851-Unity");
    }
}
#endif
