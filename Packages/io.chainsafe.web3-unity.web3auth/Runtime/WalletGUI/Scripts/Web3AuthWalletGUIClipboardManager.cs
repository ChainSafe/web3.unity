using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

/// <summary>
/// Web3Auth waller GUI clipboard manager to handle strings.
/// </summary>
public class Web3AuthWalletGUIClipboardManager : MonoBehaviour
{
    #region Fields

    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);
    
    [DllImport("__Internal")]
    private static extern void PasteFromClipboard();

    #endregion

    #region Methods
    
    /// <summary>
    /// Handles pasting on WebGl.
    /// </summary>
    /// <param name="text">Clipboard text</param>
    public void OnPasteWebGL(string text)
    {
        var inputFields = FindObjectsOfType<TMP_InputField>();
        TMP_InputField focusedInputField = inputFields.FirstOrDefault(inputField => inputField.isFocused);
        if (focusedInputField != null)
        {
            focusedInputField.text = text;
        }
    }

    /// <summary>
    /// Pastes clipboard text.
    /// </summary>
    private void PasteText()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
        {
            PasteFromClipboard();
        }
    }
    
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

    /// <summary>
    /// Polls for paste text if on WebGL.
    /// </summary>
    private void Update()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        PasteText();
        #endif
    }
    
    #endregion
}
