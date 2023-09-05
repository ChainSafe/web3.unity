#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SelectAssetInEditor : MonoBehaviour
{
    public Object Target;

    public void Select()
    {
#if UNITY_EDITOR
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = Target;
#endif
    }
}
