using System.Runtime.InteropServices;
using UnityEngine;

public class Web3AuthWalletGUIClipboardManager : MonoBehaviour
{
    #region Fields

    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);

    #endregion

    #region Methods
    
    /// <summary>
    /// Copies text from editor or browser environment to the clipboard.
    /// </summary>
    /// <param name="text"></param>
    public static void CopyText(string text)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        CopyToClipboard(text);
    #else
        GUIUtility.systemCopyBuffer = text;
    #endif
    }
    
    #endregion
}
