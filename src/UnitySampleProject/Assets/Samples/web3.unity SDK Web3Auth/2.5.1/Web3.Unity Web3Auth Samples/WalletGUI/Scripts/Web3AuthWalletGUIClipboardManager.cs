using System.Runtime.InteropServices;
using UnityEngine;

public class Web3AuthWalletGUIClipboardManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);

    public static void CopyText(string text)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
        CopyToClipboard(text);
    #else
        GUIUtility.systemCopyBuffer = text;
    #endif
    }
}
